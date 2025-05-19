import cv2
import numpy as np
import mediapipe as mp
from tensorflow.keras.models import load_model

# Danh sách hành động giống như khi train
actions = np.array([
    "brush_hair",
    "cartwheel",
    "catch",
    "chew",
    "clap",
    "climb",
    "climb_stairs",
    "dive",
    "draw_sword",
    "dribble",
    "drink",
    "eat",
    "fall_floor",
    "fencing",
    "flic_flac",
    "golf",
    "handstand",
    "hit",
    "hug",
    "jump",
    "kick",
    "kick_ball",
    "kiss",
    "laugh",
    "pick",
    "pour",
    "pullup",
    "punch",
    "push",
    "pushup",
    "ride_bike",
    "ride_horse",
    "run",
    "shake_hands",
    "shoot_ball",
    "shoot_bow",
    "shoot_gun",
    "sit",
    "situp",
    "smile",
    "smoke",
    "somersault",
    "stand",
    "swing_baseball",
    "sword",
    "sword_exercise",
    "talk",
    "throw",
    "turn",
    "walk",
    "wave"
])
sequence_length = 40

# Load model đã train
model = load_model('action_recognition_model.h5')

# Setup Mediapipe
mp_holistic = mp.solutions.holistic
mp_drawing = mp.solutions.drawing_utils

def extract_keypoints(results):
    if results.pose_landmarks:
        pose = np.array([[res.x, res.y, res.z, res.visibility] for res in results.pose_landmarks.landmark]).flatten()
    else:
        pose = np.zeros(33 * 4)
    return pose

# Video test (đổi đường dẫn cho phù hợp)
video_path = 'test_video.avi'
cap = cv2.VideoCapture(video_path)

sequence = []

with mp_holistic.Holistic(min_detection_confidence=0.5, min_tracking_confidence=0.5) as holistic:
    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break

        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        image.flags.writeable = False

        results = holistic.process(image)

        image.flags.writeable = True
        image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

        # Trích xuất keypoints
        keypoints = extract_keypoints(results)
        sequence.append(keypoints)

        # Giữ cho đủ độ dài sequence
        if len(sequence) > sequence_length:
            sequence.pop(0)

        # Khi đủ 40 frame → dự đoán
        if len(sequence) == sequence_length:
            input_data = np.expand_dims(sequence, axis=0)
            prediction = model.predict(input_data)
            predicted_action = actions[np.argmax(prediction)]
            print(f"Dự đoán: {predicted_action}")
            cv2.putText(image, predicted_action, (20, 60), cv2.FONT_HERSHEY_SIMPLEX, 1.5, (0,255,0), 3)

        # Vẽ pose
        if results.pose_landmarks:
            mp_drawing.draw_landmarks(image, results.pose_landmarks, mp_holistic.POSE_CONNECTIONS)

        cv2.imshow("Test Video", image)
        if cv2.waitKey(10) & 0xFF == ord('q'):
            break

cap.release()
cv2.destroyAllWindows()
