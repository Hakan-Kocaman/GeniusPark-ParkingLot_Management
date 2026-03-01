
from fastapi import FastAPI, File, UploadFile
from fastapi.responses import JSONResponse
from Model import Predict
import uvicorn

app = FastAPI()

@app.post("/api/plate/detect")
async def Detect(file: UploadFile = File(...)):
    try:
        success, plate, threshold, error = await Predict(file)

        return {"success": success, "plate": plate, "confidence": threshold, "error": error}
    
    except Exception as e:
        print(f"Error: {str(e)}")

        return {"success": False, "plate": None, "confidence": None, "error": str(e)}
        


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=5000)

