# test_pus
windows Install the mysql service
my.ini   configuration



[mysqld]
# Set the mysql installation directory, where you will unzip the installation package
basedir = G:\\work\\20201118\\setup\\bin\\Debug\\baseDir\\mysql-8.0.22-winx64
# Set up the mysql database data storage directory
datadir = G:\\work\\20201118\\setup\\bin\\Debug\\baseDir\\mysql-8.0.22-winx64\\data
# Set port number
port = 3306
# Maximum number of connections allowed
max_connections = 200
# The number of connection failures allowed.This is to prevent someone from trying to attack the database system from the host
max_connect_errors=20



install.bat    example



set MYSQLDIR=G:\\work\\20201118\\setup\\bin\\Debug\\baseDir\\mysql-8.0.22-winx64
set BINDIR=%MYSQLDIR%\bin
%BINDIR%\\mysqld --defaults-file=%MYSQLDIR%\\my.ini --initialize-insecure
%BINDIR%\\mysqld --install MysqlWindowsService --defaults-file=%MYSQLDIR%\\my.ini
net start MysqlWindowsService
