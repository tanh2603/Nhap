import os
import cv2
import numpy as np
import mediapipe as mp
from concurrent.futures import ThreadPoolExecutor, as_completed
from tqdm import tqdm
import random

cv2.setNumThreads(0)  # Ngăn OpenCV dùng đa luồng nội bộ

# Đường dẫn
ROOT_VIDEO_PATH = "/Users/wocten/Documents/Action_Person/hmdb51"
DATA_PATH = "DATA_TEST"
SEQUENCE_LENGTH = 30
TEST_SPLIT = 0.2  # 20% video sẽ vào tập test

# Danh sách action
actions = sorted(os.listdir(ROOT_VIDEO_PATH))

# Khởi tạo Mediapipe
mp_holistic = mp.solutions.holistic

def is_frame_too_dark(frame, threshold=10):
    """Kiểm tra frame có bị tối gần như đen hay không."""
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    return np.mean(gray) < threshold

def extract_keypoints(results, frame_width, frame_height):
    def normalize_point(x, y):
        return x / frame_width, y / frame_height
    
    # Pose
    if results.pose_landmarks:
        pose = []
        for res in results.pose_landmarks.landmark:
            x, y, z, v = res.x * frame_width, res.y * frame_height, res.z, res.visibility
            nx, ny = normalize_point(x, y)
            pose.extend([nx, ny, z, v])
        pose = np.array(pose)
    else:
        pose = np.zeros(33 * 4)

    # Face
    if results.face_landmarks:
        face = []
        for res in results.face_landmarks.landmark:
            x, y, z = res.x * frame_width, res.y * frame_height, res.z
            nx, ny = normalize_point(x, y)
            face.extend([nx, ny, z])
        face = np.array(face)
    else:
        face = np.zeros(468 * 3)

    # Left hand
    if results.left_hand_landmarks:
        lh = []
        for res in results.left_hand_landmarks.landmark:
            x, y, z = res.x * frame_width, res.y * frame_height, res.z
            nx, ny = normalize_point(x, y)
            lh.extend([nx, ny, z])
        lh = np.array(lh)
    else:
        lh = np.zeros(21 * 3)

    # Right hand
    if results.right_hand_landmarks:
        rh = []
        for res in results.right_hand_landmarks.landmark:
            x, y, z = res.x * frame_width, res.y * frame_height, res.z
            nx, ny = normalize_point(x, y)
            rh.extend([nx, ny, z])
        rh = np.array(rh)
    else:
        rh = np.zeros(21 * 3)

    return np.concatenate([pose, face, lh, rh])

# Hàm xử lý từng video
def process_video(action, video_name, video_path, save_root):
    cap = cv2.VideoCapture(video_path)
    save_dir = os.path.join(save_root, action, video_name)

    frames_keypoints = []

    with mp_holistic.Holistic(static_image_mode=False,
                              min_detection_confidence=0.5,
                              min_tracking_confidence=0.5) as holistic:
        frame_count = 0
        while frame_count < SEQUENCE_LENGTH:
            ret, frame = cap.read()
            if not ret:
                break

            # Bỏ frame quá tối
            if is_frame_too_dark(frame):
                continue

            h, w, _ = frame.shape
            image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            image.flags.writeable = False
            results = holistic.process(image)

            keypoints = extract_keypoints(results, w, h)
            frames_keypoints.append(keypoints)
            frame_count += 1

    cap.release()

    # Nếu không đủ 40 frame hợp lệ thì bỏ qua video này
    if len(frames_keypoints) < SEQUENCE_LENGTH:
        print(f"Bỏ video {video_name} (action: {action}) vì không đủ {SEQUENCE_LENGTH} frame hợp lệ ({len(frames_keypoints)} frame).")
        return

    # Lưu keypoints từng frame
    os.makedirs(save_dir, exist_ok=True)
    for i, kps in enumerate(frames_keypoints):
        np.save(os.path.join(save_dir, f"{i}.npy"), kps)

# Chạy song song toàn bộ video
def extract_all():
    with ThreadPoolExecutor(max_workers=6) as executor:
        futures = []
        for action in actions:
            action_path = os.path.join(ROOT_VIDEO_PATH, action)
            if not os.path.isdir(action_path):
                continue

            video_list = [v for v in os.listdir(action_path) if v.endswith(".avi")]
            random.shuffle(video_list)
            split_idx = int(len(video_list) * (1 - TEST_SPLIT))
            train_videos = video_list[:split_idx]
            test_videos = video_list[split_idx:]

            for video in train_videos:
                video_path = os.path.join(action_path, video)
                video_name = os.path.splitext(video)[0]
                futures.append(executor.submit(process_video, action, video_name, video_path, os.path.join(DATA_PATH, "train")))

            for video in test_videos:
                video_path = os.path.join(action_path, video)
                video_name = os.path.splitext(video)[0]
                futures.append(executor.submit(process_video, action, video_name, video_path, os.path.join(DATA_PATH, "test")))

        for _ in tqdm(as_completed(futures), total=len(futures), desc="Trích xuất keypoints"):
            pass

if __name__ == "__main__":
    extract_all()
