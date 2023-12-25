#! /bin/bash

version=$1
ram=$2
javaVer=$3

# Build
backupFolder="/Volumes/Data_1T/minecraft/BackUps/${version}"
minecraftDir="/Users/user/minecraft/servers/${version}"

# show build server info
echo -------------------------------
echo Build server info
echo
echo -e "Version\t =" $version
echo -e "RAM\t =" $ram "GB"
echo -------------------------------

# Java version
javaPath="java"

if test $javaVer -eq 17 ; then
  javaPath="/Library/Java/JavaVirtualMachines/jdk-17.0.2.jdk/Contents/Home/bin/java"
elif test $javaVer -eq 8 ; then
  javaPath="/Library/Java/JavaVirtualMachines/jdk1.8.0_202.jdk/Contents/Home/bin/java"
fi


# Build
cd $minecraftDir
sudo nice -n -10 $javaPath -Xms${ram}G -Xmx${ram}G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:+ParallelRefProcEnabled -XX:MaxGCPauseMillis=200 -XX:+DisableExplicitGC -XX:+AlwaysPreTouch -XX:G1HeapWastePercent=5 -XX:G1MixedGCCountTarget=4 -XX:G1MixedGCLiveThresholdPercent=90 -XX:G1RSetUpdatingPauseTimePercent=5 -XX:SurvivorRatio=32 -XX:+PerfDisableSharedMem -XX:MaxTenuringThreshold=1 -XX:G1NewSizePercent=40 -XX:G1MaxNewSizePercent=50 -XX:G1HeapRegionSize=16M -XX:G1ReservePercent=15 -XX:InitiatingHeapOccupancyPercent=20 -Dusing.aikars.flags=https://mcflags.emc.gs -Daikars.new.flags=true -jar server.jar nogui

########################################################
# Backup To DropBox
#
#echo
#echo "Starting Backup To DropBox."
#echo
#
#rclone sync -P $minecraftDir DropBox:Minecraft/$version 
#
# Backup To Google Drive
#echo
#echo "Starting Backup To Google Drive. "
#echo
#
#rclone sync -P /Users/user/minecraft/servers/$version GoogleDrive:RaspberryPi/Minecraft/#$version
#
########################################################

# Backup
# Create Backup Folder
mkdir -p $backupFolder
cd $backupFolder

# Archive Minecraft Directory
FolderDate=`date '+%Y%m%d%H%M'`
tar -zcvf minecraft_${version}_${FolderDate}.tar.gz $minecraftDir

#########################################################
# Copy To NAS
#
# mkdir -p /Volumes/gcus_nm/minecraft_BackUp/$version
# rsync -av --progress minecraft_${version}_${FolderDate}.tar.gz /Volumes/gcus_nm/minecraft_Backup/${version}/
########################################################

# echo
# read -p "Exit to press Enter."
cd

