import os
import numpy as np
import json
from sklearn.model_selection import train_test_split
from tensorflow.keras.models import load_model
from tensorflow.keras.callbacks import ModelCheckpoint, Callback
from model import create_model

# Cấu hình
DATA_PATH = "DATA_TEST"
SEQUENCE_LENGTH = 30
LAST_CHECKPOINT = "last_checkpoint.keras"  # ✅ đổi sang .keras

# Lấy danh sách hành động từ thư mục train
actions = sorted(os.listdir(os.path.join(DATA_PATH, "train")))
label_map = {label: idx for idx, label in enumerate(actions)}

def load_data(split):
    X, y = [], []
    split_path = os.path.join(DATA_PATH, split)

    for action in actions:
        action_dir = os.path.join(split_path, action)
        if not os.path.isdir(action_dir):
            continue

        for sequence in os.listdir(action_dir):
            sequence_path = os.path.join(action_dir, sequence)
            window = []
            for frame_num in range(SEQUENCE_LENGTH):
                frame_path = os.path.join(sequence_path, f"{frame_num}.npy")
                if os.path.exists(frame_path):
                    window.append(np.load(frame_path))
            if len(window) == SEQUENCE_LENGTH:
                X.append(window)
                y.append(label_map[action])
    
    return np.array(X), np.array(y)

# Load dữ liệu
X_train, y_train = load_data("train")
X_test, y_test = load_data("test")

print("X_train shape:", X_train.shape)
print("y_train shape:", y_train.shape)
print("X_test shape:", X_test.shape)
print("y_test shape:", y_test.shape)

# Load mô hình nếu đã có checkpoint, ngược lại thì tạo mới
if os.path.exists(LAST_CHECKPOINT):
    print(f"Đang tải mô hình từ {LAST_CHECKPOINT} để tiếp tục huấn luyện...")
    model = load_model(LAST_CHECKPOINT)
else:
    print("Không tìm thấy checkpoint. Khởi tạo mô hình mới.")
    model = create_model(actions)

# Biên dịch mô hình
model.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])

# Callback lưu mô hình mỗi 10 epoch
class SaveEveryNEpochs(Callback):
    def __init__(self, every=10, save_path_template="action_recognition_model_epoch_{epoch}.keras"):
        super().__init__()
        self.every = every
        self.save_path_template = save_path_template

    def on_epoch_end(self, epoch, logs=None):
        if (epoch + 1) % self.every == 0:
            save_path = self.save_path_template.format(epoch=epoch + 1)
            self.model.save(save_path)
            print(f"\n✅ Đã lưu checkpoint: {save_path}")

# Callback luôn lưu checkpoint cuối cùng
last_checkpoint_callback = ModelCheckpoint(
    filepath=LAST_CHECKPOINT,
    save_weights_only=False,
    save_best_only=False,
    verbose=1
)

# Huấn luyện
history = model.fit(
    X_train, y_train,
    epochs=50,
    batch_size=32,
    validation_data=(X_test, y_test),
    callbacks=[SaveEveryNEpochs(every=10), last_checkpoint_callback]
)

# Lưu lịch sử huấn luyện
with open("training_history.json", "w") as f:
    json.dump(history.history, f)

# Lưu độ chính xác cuối cùng
with open("test_accuracy.txt", "w") as f:
    f.write(str(history.history["val_accuracy"][-1]))

print("✅ Huấn luyện hoàn tất.")
