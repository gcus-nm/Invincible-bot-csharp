#! /bin/bash

# Default Setting
version=1.18.1P
ram=6

# directory move
cd
cd minecraft/servers

# select build server version
echo Choose build server version.
echo
echo Enable server version
echo -------------------------------
ls
echo -------------------------------

read version
cd $version
echo

# select usage RAM
echo "Specify the amount of memory to allocate (recommended is 6GB)."
echo
echo Usage RAM
echo -------------------------------
free -h
echo -------------------------------
read ram

echo

# build with GUI
read -p "Do you want to display the GUI? (y/n) : " yn
echo

# show build server info
echo -------------------------------
echo Build server info
echo
echo -e "Version\t =" $version
echo -e "RAM\t =" $ram "GB"

# set CPU frequency max
sudo cpufreq-set -g performance

# build server
case "$yn" in
	[yY]*)
		echo -e "GUI\t = enable"
		echo -------------------------------
		echo
		sudo nice -n -20 java -Xmx${ram}G -XX:+UnlockExperimentalVMOptions -XX:+UseZGC -jar server.jar
		;;
	*)
		echo -e "GUI\t = disable"

		echo -------------------------------
		echo
		sudo nice -n -20 java -Xmx${ram}G -XX:+UnlockExperimentalVMOptions -XX:+UseZGC -jar server.jar nogui
		;;
esac

# set CPU frequency ondemand
sudo cpufreq-set -g ondemand

# Backup To Google Drive
cd /home/pi/minecraft/servers
rclone sync -P $version GoogleDrive:RaspberryPi/Minecraft/$version

echo
read -p "Exit to press Enter."
