from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import LSTM, Dense, Dropout, BatchNormalization

def create_model(actions):
    model = Sequential()
    
    # 3 lớp LSTM với dropout
    model.add(LSTM(128, return_sequences=True, activation='relu', input_shape=(30, 1662)))
    model.add(Dropout(0.2))
    model.add(LSTM(256, return_sequences=True, activation='relu'))
    model.add(Dropout(0.2))
    model.add(LSTM(256, return_sequences=False, activation='relu'))
    model.add(BatchNormalization())
    
    # Các lớp fully connected
    model.add(Dense(256, activation='relu'))
    model.add(Dense(128, activation='relu'))
    model.add(Dense(64, activation='relu'))
    
    # Lớp đầu ra (số lớp bằng số hành động)
    model.add(Dense(len(actions), activation='softmax'))

    model.summary()
    return model
