import os
import numpy as np
import cv2
import mediapipe as mp
from tensorflow.keras.models import load_model
from collections import deque
import imageio  # để lưu GIF

VIDEO_FOLDER = "video_inputs"
MODEL_PATH = "action_recognition_model.h5"
sequence_length = 30

actions = np.array([
    "brush_hair", "cartwheel", "catch", "chew", "clap", "climb",
    "climb_stairs", "dive", "draw_sword", "dribble", "drink", "eat",
    "fall_floor", "fencing", "flic_flac", "golf", "handstand", "hit",
    "hug", "jump", "kick", "kick_ball", "kiss", "laugh", "pick", "pour",
    "pullup", "punch", "push", "pushup", "ride_bike", "ride_horse",
    "run", "shake_hands", "shoot_ball", "shoot_bow", "shoot_gun", "sit",
    "situp", "smile", "smoke", "somersault", "stand", "swing_baseball",
    "sword", "sword_exercise", "talk", "throw", "turn", "walk", "wave"
])

mp_holistic = mp.solutions.holistic
mp_drawing = mp.solutions.drawing_utils

def extract_keypoints(results):
    if results.pose_landmarks:
        pose = np.array([[res.x, res.y, res.z, res.visibility] for res in results.pose_landmarks.landmark]).flatten()
    else:
        pose = np.zeros(33 * 4)
    if results.face_landmarks:
        face = np.array([[res.x, res.y, res.z] for res in results.face_landmarks.landmark]).flatten()
    else:
        face = np.zeros(468 * 3)
    if results.left_hand_landmarks:
        left_hand = np.array([[res.x, res.y, res.z] for res in results.left_hand_landmarks.landmark]).flatten()
    else:
        left_hand = np.zeros(21 * 3)
    if results.right_hand_landmarks:
        right_hand = np.array([[res.x, res.y, res.z] for res in results.right_hand_landmarks.landmark]).flatten()
    else:
        right_hand = np.zeros(21 * 3)
    return np.concatenate([pose, face, left_hand, right_hand])

def main():
    model = load_model(MODEL_PATH)
    video_files = [f for f in os.listdir(VIDEO_FOLDER) if f.endswith('.avi')]

    with mp_holistic.Holistic(min_detection_confidence=0.5, min_tracking_confidence=0.5) as holistic:
        for video_file in video_files:
            video_path = os.path.join(VIDEO_FOLDER, video_file)
            cap = cv2.VideoCapture(video_path)
            print(f"Đang xử lý video: {video_file}")

            sequence = deque(maxlen=sequence_length)
            predictions = []

            frames_for_gif = []  # Lưu frame để tạo GIF

            while cap.isOpened():
                ret, frame = cap.read()
                if not ret:
                    break

                image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                image.flags.writeable = False
                results = holistic.process(image)
                image.flags.writeable = True
                image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

                # Vẽ landmarks
                if results.pose_landmarks:
                    mp_drawing.draw_landmarks(image, results.pose_landmarks, mp_holistic.POSE_CONNECTIONS)
                if results.face_landmarks:
                    mp_drawing.draw_landmarks(image, results.face_landmarks, mp_holistic.FACEMESH_TESSELATION)
                if results.left_hand_landmarks:
                    mp_drawing.draw_landmarks(image, results.left_hand_landmarks, mp_holistic.HAND_CONNECTIONS)
                if results.right_hand_landmarks:
                    mp_drawing.draw_landmarks(image, results.right_hand_landmarks, mp_holistic.HAND_CONNECTIONS)

                keypoints = extract_keypoints(results)
                sequence.append(keypoints)

                action_text = ""
                if len(sequence) == sequence_length:
                    input_data = np.expand_dims(sequence, axis=0)
                    prediction = model.predict(input_data, verbose=0)
                    action_idx = np.argmax(prediction)
                    action_text = actions[action_idx]
                    predictions.append(action_text)

                if action_text:
                    cv2.putText(image, f'Action: {action_text}', (10, 40),
                                cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2, cv2.LINE_AA)

                frames_for_gif.append(cv2.cvtColor(image, cv2.COLOR_BGR2RGB))  # imageio dùng RGB

                cv2.imshow(f'Video: {video_file}', image)
                if cv2.waitKey(25) & 0xFF == ord('q'):
                    print("Dừng bởi người dùng")
                    break

            cap.release()
            cv2.destroyAllWindows()

            # Lưu GIF
            gif_path = os.path.join(VIDEO_FOLDER, f"{os.path.splitext(video_file)[0]}_predicted.gif")
            print(f"Lưu GIF dự đoán tại: {gif_path}")
            imageio.mimsave(gif_path, frames_for_gif, fps=30)

            # In kết quả tổng hợp
            if predictions:
                from collections import Counter
                most_common_action = Counter(predictions).most_common(1)[0][0]
                print(f"Kết quả dự đoán cho video {video_file}: {most_common_action}")
            else:
                print(f"Không đủ dữ liệu để dự đoán video {video_file}")

if __name__ == "__main__":
    main()
