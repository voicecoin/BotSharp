# Voiceweb decentralized chatbot (Voicebot)

We're building a voice-driven decentralized smart web based on voicechain. 
The universal bridge between AI chatbot and IoT.

```shell
cd ~/voiceweb_chatbot_core_src/Voicebot.WebStarter/
dotnet build
nohup dotnet bin/Debug/netcoreapp2.0/Voicebot.WebStarter.dll </dev/null> voicebot.log 2>&1 &

nohup python -m rasa_nlu.server --path projects &

```