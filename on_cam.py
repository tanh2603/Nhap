import tkinter as tk
from tkinter import filedialog
from PIL import Image, ImageTk
import cv2
import numpy as np

# Giả sử hàm dự đoán đã được định nghĩa sẵn, hoặc bạn có thể cài phần này theo mô hình bạn train
def predict_action(frame):
    """
    Dummy function: nhận frame BGR, trả về action và confidence.
    Thực tế bạn gọi model keras predict keypoints ở đây rồi trả kết quả.
    """
    # Ví dụ tạm: giả sử luôn trả action 'walk' với confidence 0.85
    return "walk", 0.85

class CameraApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Camera Interface")
        
        self.video_label = tk.Label(root)
        self.video_label.pack()

        self.status_label = tk.Label(root, text="Trạng thái: Tắt")
        self.status_label.pack()

        self.btn_start = tk.Button(root, text="Bật Camera", command=self.start_camera)
        self.btn_start.pack(pady=5)

        self.btn_stop = tk.Button(root, text="Tắt Camera/Video", command=self.stop_camera)
        self.btn_stop.pack(pady=5)

        self.btn_upload = tk.Button(root, text="Tải Video Test", command=self.upload_video)
        self.btn_upload.pack(pady=5)

        self.cap = None
        self.running = False

    def start_camera(self):
        self.stop_camera()  # Tắt nguồn hiện tại nếu có
        self.cap = cv2.VideoCapture(0, cv2.CAP_AVFOUNDATION)  # Mở camera tích hợp Mac
        if not self.cap.isOpened():
            print("Không thể mở camera FaceTime HD")
            self.status_label.config(text="Trạng thái: Lỗi mở camera")
            return
        self.cap.set(cv2.CAP_PROP_FPS, 60)
        self.running = True
        self.status_label.config(text="Trạng thái: Camera đang bật")
        self.update_frame()

    def stop_camera(self):
        self.running = False
        if self.cap:
            self.cap.release()
            self.cap = None
        self.video_label.config(image='')
        self.status_label.config(text="Trạng thái: Đã tắt")

    def upload_video(self):
        self.stop_camera()  # Tắt nguồn hiện tại nếu có
        filepath = filedialog.askopenfilename(filetypes=[("Video files", "*.mp4;*.avi")])
        if filepath:
            self.cap = cv2.VideoCapture(filepath)
            if not self.cap.isOpened():
                print("Không thể mở file video")
                self.status_label.config(text="Trạng thái: Lỗi mở video")
                return
            self.running = True
            self.status_label.config(text=f"Trạng thái: Đang phát video {filepath.split('/')[-1]}")
            self.update_frame()

    def update_frame(self):
        if self.running and self.cap and self.cap.isOpened():
            ret, frame = self.cap.read()
            if ret:
                frame = cv2.flip(frame, 1)  # lật ngang
                # Dự đoán action & confidence
                action, conf = predict_action(frame)

                # Vẽ action lên frame
                cv2.putText(frame, f"Action: {action}", (10, 30), 
                            cv2.FONT_HERSHEY_SIMPLEX, 1, (0,255,0), 2, cv2.LINE_AA)
                
                # Vẽ thanh progress bar confidence
                bar_x, bar_y, bar_w, bar_h = 10, 50, 300, 20
                cv2.rectangle(frame, (bar_x, bar_y), (bar_x + bar_w, bar_y + bar_h), (255,255,255), 2)
                fill_w = int(bar_w * conf)
                cv2.rectangle(frame, (bar_x, bar_y), (bar_x + fill_w, bar_y + bar_h), (0,255,0), -1)
                cv2.putText(frame, f"{int(conf*100)}%", (bar_x + bar_w + 10, bar_y + bar_h - 5), 
                            cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0,255,0), 2)

                frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                img = Image.fromarray(frame)
                imgtk = ImageTk.PhotoImage(image=img)
                self.video_label.imgtk = imgtk
                self.video_label.configure(image=imgtk)
                self.root.after(10, self.update_frame)
            else:
                self.stop_camera()


if __name__ == "__main__":
    root = tk.Tk()
    app = CameraApp(root)
    root.mainloop()
