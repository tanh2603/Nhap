import tkinter as tk
from tkinter import filedialog
from PIL import Image, ImageTk
import cv2
import numpy as np
from collections import deque
from tensorflow.keras.models import load_model

class CameraApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Camera và Nhận diện hành động")

        self.video_label = tk.Label(root)
        self.video_label.pack()

        self.action_label = tk.Label(root, text="Hành động: ", font=("Arial", 20))
        self.action_label.pack()

        self.btn_start = tk.Button(root, text="Bật Camera", command=self.start_camera)
        self.btn_start.pack(pady=5)

        self.btn_stop = tk.Button(root, text="Tắt Camera", command=self.stop_camera)
        self.btn_stop.pack(pady=5)

        self.btn_upload = tk.Button(root, text="Tải Video Test", command=self.upload_video)
        self.btn_upload.pack(pady=5)

        self.cap = None
        self.running = False

        # Load model và nhãn
        self.model = load_model("action_recognition_model.h5")
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

        self.sequence_length = 40
        self.frame_queue = deque(maxlen=self.sequence_length)

    def start_camera(self):
        self.cap = cv2.VideoCapture(0, cv2.CAP_AVFOUNDATION)
        if not self.cap.isOpened():
            print("Không mở được camera")
            return
        self.running = True
        self.frame_queue.clear()
        self.update_frame()

    def stop_camera(self):
        self.running = False
        if self.cap:
            self.cap.release()
            self.video_label.config(image='')
            self.action_label.config(text="Hành động: ")

    def upload_video(self):
        filepath = filedialog.askopenfilename(filetypes=[("Video files", "*.mp4;*.avi;*.mov")])
        if filepath:
            print("Đã chọn video:", filepath)
            if self.cap:
                self.cap.release()
            self.cap = cv2.VideoCapture(filepath)
            if not self.cap.isOpened():
                print("Không mở được video:", filepath)
                self.cap = None
                self.running = False
                return
            self.running = True
            self.frame_queue.clear()
            self.update_frame()
        else:
            print("Không chọn video nào")


    def preprocess_frame(self, frame):
        resized_frame = cv2.resize(frame, (100, 100))
        gray = cv2.cvtColor(resized_frame, cv2.COLOR_BGR2GRAY)
        normalized = gray / 255.0
        return normalized

    def update_frame(self):
        if self.running and self.cap and self.cap.isOpened():
            ret, frame = self.cap.read()
            if not ret:
                self.stop_camera()
                return

            # Flip nếu bạn muốn (camera ngược)
            frame = cv2.flip(frame, 1)

            # Chuẩn bị frame cho model
            processed = self.preprocess_frame(frame)
            self.frame_queue.append(processed)

            if len(self.frame_queue) == self.sequence_length:
                input_data = np.array(self.frame_queue)
                input_data = np.expand_dims(input_data, axis=0)       # (1, 40, 100, 100)
                input_data = np.expand_dims(input_data, axis=-1)      # (1, 40, 100, 100, 1)

                prediction = self.model.predict(input_data)[0]
                predicted_label = self.actions[np.argmax(prediction)]
                self.action_label.config(text=f"Hành động: {predicted_label}")
            else:
                self.action_label.config(text="Hành động: Đang xử lý...")

            # Hiển thị video
            frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            img = Image.fromarray(frame_rgb)
            imgtk = ImageTk.PhotoImage(image=img)
            self.video_label.imgtk = imgtk
            self.video_label.configure(image=imgtk)

            self.root.after(60, self.update_frame)

if __name__ == "__main__":
    root = tk.Tk()
    app = CameraApp(root)
    root.mainloop()
