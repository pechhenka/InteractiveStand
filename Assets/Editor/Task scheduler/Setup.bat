powercfg -h off
cd %~dp0
schtasks /Create /XML "InteractiveStand_ON.xml" /tn InteractiveStand_ON
schtasks /Create /XML "InteractiveStand_OFF.xml" /tn InteractiveStand_OFF
pause