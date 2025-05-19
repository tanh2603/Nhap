import numpy as np
import os
import json
from model import create_model
from sklearn.model_selection import train_test_split

# Đường dẫn dữ liệu
DATA_PATH = os.path.join("DATA_TEST")

# Danh sách actions
actions = np.array([
    "brush_hair", "cartwheel", "catch", "chew", "clap", "climb", "climb_stairs", "dive", "draw_sword",
    "dribble", "drink", "eat", "fall_floor", "fencing", "flic_flac", "golf", "handstand", "hit", "hug",
    "jump", "kick", "kick_ball", "kiss", "laugh", "pick", "pour", "pullup", "punch", "push", "pushup",
    "ride_bike", "ride_horse", "run", "shake_hands", "shoot_ball", "shoot_bow", "shoot_gun", "sit",
    "situp", "smile", "smoke", "somersault", "stand", "swing_baseball", "sword", "sword_exercise",
    "talk", "throw", "turn", "walk", "wave"
])

X = []
y = []

# Đọc dữ liệu từ file .npy
for action in actions:
    for sequence in range(1775):
        window = []
        for frame_num in range(40):
            file_path = os.path.join(DATA_PATH, action, str(sequence), f"{frame_num}.npy")
            if os.path.exists(file_path):
                window.append(np.load(file_path))
        if len(window) == 40:
            X.append(window)
            y.append(actions.tolist().index(action))

X = np.array(X)
y = np.array(y)

print("X shape:", X.shape)  # (samples, 40, feature_dim)
print("y shape:", y.shape)

# Chia dữ liệu train/test
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Tạo mô hình
model = create_model(actions)
model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])

# Huấn luyện mô hình và lưu lịch sử
history = model.fit(X_train, y_train, epochs=200, batch_size=32, validation_data=(X_test, y_test))

# Lưu mô hình
model.save("action_recognition_model.h5")

# Lưu lịch sử huấn luyện ra file JSON để dùng hiển thị sau
history_dict = history.history
with open('training_history.json', 'w') as f:
    json.dump(history_dict, f)

# Đánh giá trên tập test
loss, accuracy = model.evaluate(X_test, y_test)
print(f"Test Loss: {loss:.4f}")
print(f"Test Accuracy: {accuracy:.4f}")

# Lưu test accuracy ra file
with open('test_accuracy.txt', 'w') as f:
    f.write(str(accuracy))
