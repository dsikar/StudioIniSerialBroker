#!/bin/bash

# Change access so www-data user may read access log.

sudo chmod 755 /var/log/apache2
sudo chmod 755 /var/log/apache2/access.log

# Change access so www-data can write to /tmp/*log.txt

sudo chmod 777 /tmp/1log.txt
sudo chmod 777 /tmp/2log.txt

# to add more sensors, create anoter log file e.g. 
# sudo chmod 777 /tmp/3log.txt

#and rerun this script i.e.
# $ . /var/www/html/access_log.sh
