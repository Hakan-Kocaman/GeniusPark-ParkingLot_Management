


async def Predict(file):
    plate= file.filename
    success = True
    threshold = 0.7  # Example threshold value
    error= None
    return success, plate, threshold, error