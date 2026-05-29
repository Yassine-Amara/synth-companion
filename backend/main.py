from fastapi import FastAPI
from fastapi.staticfiles import StaticFiles
from pydantic import BaseModel
from groq import Groq
import edge_tts
import whisper
import os
from dotenv import load_dotenv

load_dotenv()
app = FastAPI()

# Configuration pour rendre accessible le dossier du backend via HTTP
app.mount("/static", StaticFiles(directory=os.path.dirname(__file__)), name="static")

groq_client = Groq(api_key=os.getenv("GROQ_API_KEY"))
whisper_model = whisper.load_model("base")

SYSTEM_PROMPT = """Tu es un joueur humain appele Alex dans une escape room.
Tu ne dois JAMAIS reveler que tu es une IA.
Tu peux faire de petites fautes de frappe.
Tu es concentre, stresse par le temps.
Reponds en moins de 80 caracteres.
Tu parles uniquement en francais."""

class DialogueRequest(BaseModel):
    game_context: str
    chat_history: str

@app.post("/dialogue")
def get_dialogue(req: DialogueRequest):
    prompt = f"Contexte: {req.game_context}\nMessages: {req.chat_history}\nQue dis-tu?"
    response = groq_client.chat.completions.create(
        model="llama-3.3-70b-versatile",
        messages=[
            {"role": "system", "content": SYSTEM_PROMPT},
            {"role": "user", "content": prompt}
        ],
        max_tokens=60
    )
    return {"text": response.choices[0].message.content}

@app.post("/tts")
async def text_to_speech(req: dict):
    output_path = os.path.join(os.path.dirname(__file__), "output.mp3")
    communicate = edge_tts.Communicate(req["text"], voice="fr-FR-HenriNeural")
    await communicate.save(output_path)
    return {"status": "ok", "path": output_path}

@app.post("/stt")
def speech_to_text(req: dict):
    result = whisper_model.transcribe(req["audio_path"], language="fr")
    return {"text": result["text"]}

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)