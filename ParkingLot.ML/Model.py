from ultralytics import YOLO
import numpy as np
import cv2
import easyocr


model = YOLO("license_plate_model.pt")

async def Predict(file):
    try:
        contents = await file.read()
        nparr = np.frombuffer(contents, np.uint8)
        img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

        results = model()
        
        reader = easyocr.Reader(['en'])

        for result in results:
            for box in result.boxes:
                x1, y1, x2, y2 = map(int, box.xyxy[0])
                cropped_img = img[y1:y2, x1:x2]

                ocr_result = reader.readtext(cropped_img)

                if ocr_result:
                    plate_text = ocr_result[0][1]
                    confidence = ocr_result[0][2]
                    return True, plate_text, confidence, None

        return False, None, None, "No license plate detected"
    
    except Exception as e:
        return False, None, None, str(e)
