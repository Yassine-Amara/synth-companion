# 🎮🥽 Synth-Companion — Escape Room avec IA projet 6

> **Projet étudiant**   
> Unity 6.3 LTS | Python | Groq (Llama 3) 

---

## 🧠 Concept

Une salle d'évasion multijoueur où une équipe de joueurs doit résoudre 3 puzzles collaboratifs.  
Un des joueurs est en réalité une **IA (Alex)** qui se fait passer pour un humain dans le chat.  
Les joueurs doivent détecter qui est l'IA avant la fin de la partie.

---

## 🎯 Les 3 Puzzles

| Puzzle | Description |
|--------|-------------|
| 🔐 Code | Trouver et entrer le bon code |
| 🔵 Balle | Glisser la balle rouge vers la zone cible bleu |
| 🔴 Séquence | Appuyer sur 4 boutons colorés dans le bon ordre : Rouge → Vert → Bleu → Jaune |

---

## 🛠️ Prérequis à installer

Chaque membre du groupe doit installer ces outils sur son PC.

### 1. Unity 6.3 LTS
- Va sur https://unity.com
- Télécharge **Unity Hub**
- Installe **Unity 6.3 LTS** avec ces modules :
  - ✅ Windows Build Support
  - ✅ Android Build Support
  - ✅ Android SDK & NDK Tools
  - ✅ Visual Studio Community 2022

### 2. Python 3.11
- Va sur https://www.python.org/downloads/
- **IMPORTANT** : Coche **"Add Python to PATH"** lors de l'installation

### 3. FFmpeg
- Va sur https://www.gyan.dev/ffmpeg/builds/
- Télécharge `ffmpeg-release-full.7z`
- Extrais et renomme le dossier en `ffmpeg`
- Colle-le dans `C:\ffmpeg`
- Ajoute `C:\ffmpeg\bin` dans les **variables d'environnement PATH** :
  - Touche Windows → tape "variables d'environnement"
  - Variables système → Path → Modifier → Nouveau → colle `C:\ffmpeg\bin`
  - OK → OK → Appliquer
- Vérifie dans un nouveau terminal :
```
ffmpeg -version
```

### 4. Git
- Va sur https://git-scm.com/downloads

---

## 🚀 Installation du projet

### Étape 1 — Cloner le projet
```bash
git clone https://github.com/Yassine-Amara/synth-companion.git
cd synth-companion
```

### Étape 2 — Installer les packages Python
```bash
pip install fastapi uvicorn groq edge-tts openai-whisper python-dotenv
```

### Étape 3 — Créer ta clé Groq (OBLIGATOIRE)
- Va sur https://groq.com → **Start Building** → crée un compte gratuit
- Va dans **API Keys** → **Create API Key** → copie la clé
- Dans le dossier `backend/`, crée un fichier `.env` :
```
GROQ_API_KEY=gsk_TA_PROPRE_CLE
```
> ⚠️ Ce fichier n'est PAS sur GitHub. Chaque membre doit créer le sien.

### Étape 4 — Ouvrir dans Unity
- Ouvre **Unity Hub**
- Clique **Open** → navigue vers le dossier `synth-companion` cloné
- Attends la compilation initiale (5-10 min)

---

## ▶️ Lancer le jeu

### Étape 1 — Lancer le backend Python
Ouvre un terminal et tape :
```bash
cd synth-companion/backend
python main.py
```
Tu dois voir :
```
Uvicorn running on http://0.0.0.0:8000
```
> ⚠️ Laisse ce terminal ouvert pendant toute la partie

### Étape 2 — Lancer le jeu dans Unity
- Ouvre la scène `Assets/Scenes/GameRoom`
- Appuie sur **Play**
- Clique **HOST** pour créer une partie

### Étape 3 — Rejoindre avec un deuxième joueur
**Option A — ParrelSync (même PC)**
- Menu Unity → **ParrelSync > Clone Manager** → **Open in New Editor**
- Dans le clone → Play → **JOIN**

**Option B — Build Windows (deux PCs)**
- File > Build Settings → Windows → Build and Run
- Lance la build sur le deuxième PC → **JOIN**

---

## 🎮 Contrôles

### Mode Desktop (clavier/souris)

| Touche | Action |
|--------|--------|
| W A S D | Se déplacer |
| Souris | Regarder autour |
| Échap | Libérer la souris pour cliquer |
| T (maintenir) | Parler au micro (Speech-to-Text) |

1. Lancer Unity → appuyer **Play**
2. Cliquer **Host** pour créer la partie
3. L'autre joueur clique **Join** pour rejoindre

### Mode VR (Meta Quest 2)

| Contrôle | Action |
|----------|--------|
| Joystick gauche | Se déplacer |
| Joystick droit | Tourner |
| Gâchette (index) | Interagir / Grab |
| Bouton X | Parler (reconnaissance vocale) |

**Prérequis VR :**
1. Brancher le Quest 2 via câble USB
2. Activer **Quest Link** dans le casque
3. Accepter la permission de débogage
4. Lancer Unity → **Play** → **Host** ou **Join**
---

## 🗂️ Structure du projet

```
synth-companion/
├── Assets/
│   ├── Scripts/
│   │   ├── Network/       → NetworkManagerSetup.cs, DesktopPlayer.cs, ChatManager.cs
│   │   ├── AI/            → AIPlayer.cs, LLMConnector.cs
│   │   ├── Puzzles/       → PuzzleManager.cs, KeypadPuzzle.cs, PlacementPuzzle.cs
│   │   │                     SequencePuzzle.cs, PuzzleButton.cs, BallDragger.cs, PuzzleHUD.cs
│   │   ├── Data/          → SessionLogger.cs
│   │   ├── VR/            → TTSPlayer.cs, VoiceCapture.cs, SavWav.cs
│   │   └── GameController.cs
│   │   
│   ├── Prefabs/           → DesktopPlayer, AIPlayer, Ball, VRPlayer
│   └── Scenes/            → GameRoom
├── backend/
│   ├── main.py            → Serveur FastAPI + Groq + TTS + STT
│   ├── analyse.py         → Analyse des données comportementales
│   └── .env               → ⚠️ A créer soi-même (clé Groq)
└── README.md
```

---

## 🔧 Outils utilisés — 100% Gratuit

| Outil | Usage | Lien |
|-------|-------|------|
| Unity 6.3 LTS | Moteur de jeu | unity.com |
| Python 3.11 | Backend API | python.org |
| Groq (Llama 3) | LLM gratuit | groq.com |
| Edge TTS | Synthèse vocale | pip install edge-tts |
| Whisper local | Reconnaissance vocale | pip install openai-whisper |
| FFmpeg | Audio processing | gyan.dev/ffmpeg |
| FastAPI | Serveur API | pip install fastapi |
| ParrelSync | Test multijoueur | Package Manager Unity |
| Git + GitHub | Versioning | github.com |

---

## ❗ Problèmes courants

- Le serveur backend doit être lancé **avant** Unity pour que la voix et l'IA fonctionnent
- En mode VR, le chat textuel n'est pas disponible — utiliser la voix (bouton X)
- FFmpeg doit être dans le PATH sinon Whisper ne fonctionne pas


---

## 📊 Analyse des données

Après une session de jeu, les données sont sauvegardées dans :
```
C:\Users\TON_NOM\AppData\LocalLow\DefaultCompany\SynthCompanion\
```

Pour analyser :
```bash
cd backend
python analyse.py
```
Génère un graphique `resultats.png` avec les actions par joueur.

---

## 👥 Équipe

## 👥 Équipe et Contributions

| Membre | Rôle | Contribution |
|--------|------|-------------|
| AMARA YASSINE | Lead Developer & Architecte réseau | Réseau multijoueur, synchronisation, architecture, DesktopPlayer (mouvement/caméra), NetworkManagerSetup, intégration VR, fix ParrelSync, configuration XR Interaction Toolkit, ChatManager, interface Canvas (LobbyPanel, ChatPanel, CodePanel), SequenceProgress, GameTimerText, disposition des boutons keypad |
| Khachane Zakaria | Développeur IA & Backend | Serveur FastAPI Python, intégration Groq LLM, personnage Alex (AIPlayer), LLMConnector, SessionLogger, GameController |
| Serrar Zouhair | Développeur Gameplay & Physique | Puzzle 2 BallDragger (drag souris + VR grab), PlacementPuzzle, Target, XR Grab Interactable sur la balle |
| Hajar Oussama | Développeur Audio & Voix |  VoiceCapture (STT Whisper), TTSPlayer (Edge TTS), SavWav, intégration micro Quest 2, support bouton X VR |
| Hajji Imrane | Développeur Puzzles | Puzzle 1 KeypadPuzzle (code numérique, boutons chiffres), Puzzle 3 SequencePuzzle (cubes colorés, ordre séquence), PuzzleButton, PuzzleManager |
| Naqi Boubker | Scène 3D & Intégration | Conception scène GameRoom (murs, torches, éclairage), placement puzzles dans la scène, matériaux, préfabs, tests finaux |
| Benouarrak Abdelbari | QA & Documentation | Tests multijoueur (ParrelSync), rapport de bugs, rédaction du README, documentation du projet, tests de compatibilité Desktop/VR, validation des puzzles |


---

## 📄 Licence

Projet étudiant — Usage académique uniquement.
