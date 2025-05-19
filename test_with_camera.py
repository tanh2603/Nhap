import cv2
import numpy as np
from tensorflow.keras.models import load_model

# Cấu hình
FRAME_SIZE = (112, 112)
SEQUENCE_LENGTH = 40
LABELS = [
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
]

model = load_model('models/action_recognition_model.keras')

def preprocess_frame(frame):
    frame = cv2.resize(frame, FRAME_SIZE)
    frame = frame / 255.0
    return frame

cap = cv2.VideoCapture(0)
sequence = []

print("Đang nhận diện hành động (camera)... Nhấn 'q' để thoát.")

while True:
    ret, frame = cap.read()
    if not ret:
        break

    processed = preprocess_frame(frame)
    sequence.append(processed)

    if len(sequence) == SEQUENCE_LENGTH:
        input_data = np.expand_dims(sequence, axis=0)
        predictions = model.predict(input_data, verbose=0)[0]
        predicted_class = np.argmax(predictions)
        confidence = predictions[predicted_class]
        action_name = LABELS[predicted_class]

        cv2.putText(frame, f'Action: {action_name} ({confidence:.2f})', (10, 30),
                    cv2.FONT_HERSHEY_SIMPLEX, 0.9, (0, 255, 0), 2)
        sequence.pop(0)

    cv2.imshow('Webcam Action Recognition', frame)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
