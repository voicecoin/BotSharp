# Voiceweb decentralized chatbot (Voicebot)

We're building a voice-driven decentralized smart web based on voicechain. 
The universal bridge between AI chatbot and IoT.

```shell

pkill -f Voicebot.WebStarter
cd /var/www/voicebot.pro
nohup dotnet Voicebot.WebStarter.dll </dev/null> App_Data/Logs/voicebot.log 2>&1 &
ps aux | grep Voicebot.WebStarter

nohup python -m rasa_nlu.server --path projects &

```