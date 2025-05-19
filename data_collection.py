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
SEQUENCE_LENGTH = 40
TEST_SPLIT = 0.2  # 20% video sẽ vào tập test

# Danh sách action
actions = sorted(os.listdir(ROOT_VIDEO_PATH))

# Khởi tạo Mediapipe
mp_holistic = mp.solutions.holistic

# Hàm trích xuất keypoints
def extract_keypoints(results):
    # Pose
    if results.pose_landmarks:
        pose = np.array([[res.x, res.y, res.z, res.visibility] for res in results.pose_landmarks.landmark]).flatten()
    else:
        pose = np.zeros(33 * 4)

    # Face
    if results.face_landmarks:
        face = np.array([[res.x, res.y, res.z] for res in results.face_landmarks.landmark]).flatten()
    else:
        face = np.zeros(468 * 3)

    # Left hand
    if results.left_hand_landmarks:
        lh = np.array([[res.x, res.y, res.z] for res in results.left_hand_landmarks.landmark]).flatten()
    else:
        lh = np.zeros(21 * 3)

    # Right hand
    if results.right_hand_landmarks:
        rh = np.array([[res.x, res.y, res.z] for res in results.right_hand_landmarks.landmark]).flatten()
    else:
        rh = np.zeros(21 * 3)

    return np.concatenate([pose, face, lh, rh])

# Hàm xử lý từng video
def process_video(action, video_name, video_path, save_root):
    cap = cv2.VideoCapture(video_path)
    save_dir = os.path.join(save_root, action, video_name)
    os.makedirs(save_dir, exist_ok=True)

    with mp_holistic.Holistic(static_image_mode=False,
                              min_detection_confidence=0.5,
                              min_tracking_confidence=0.5) as holistic:
        frame_count = 0
        while frame_count < SEQUENCE_LENGTH:
            ret, frame = cap.read()
            if not ret:
                break

            image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            image.flags.writeable = False
            results = holistic.process(image)

            keypoints = extract_keypoints(results)
            np.save(os.path.join(save_dir, f"{frame_count}.npy"), keypoints)

            frame_count += 1

    cap.release()

# Chạy song song toàn bộ video
def extract_all():
    jobs = []
    with ThreadPoolExecutor(max_workers=6) as executor:
        futures = []
        for action in actions:
            action_path = os.path.join(ROOT_VIDEO_PATH, action)
            if not os.path.isdir(action_path): continue

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

        # Hiển thị tiến độ
        for _ in tqdm(as_completed(futures), total=len(futures), desc="Trích xuất keypoints"):
            pass

# Chạy chính
if __name__ == "__main__":
    extract_all()
