
mkdir ResourceTemp/;
for i in `ls *.png`;
do /Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/usr/bin/pngcrush -revert-iphone-optimizations $i ResourceTemp/$i;
done