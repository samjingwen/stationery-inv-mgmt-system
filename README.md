# Logic University Stationery Store Inventory System

## 1) Installation Guide

### Configuration for Web API and Web Application

1.	Restore the database (adteam7db.bak) file that is located inside "DatabaseBackup" folder in the project solution directory. (Team7ADProject\DatabaseBackup)
2.	Open Team7ADProject solution with Visual Studio. Check that the "connectionString" of the Web.config files of projects Team7ADProject and Team7ADProjectAPI is pointing to the local database server. Include any SQL Authentication if necessary.
3.	Run Visual Studio as Administrator and publish the project “Team7ADProjectAPI”.
4.	Right click on "Team7ADProject" and select "View" then click “View in Browser”.

### Configuration for Android

1.	Open the android source code with android studio and go to “app/java/data/ReturntoWarehouseApi” and “app/java/network/APIDataAgentImpl”.
2.	Change the String “host” to the IP address of the published “Team7ADProjectAPI” or change to whatever URL the Web API Application is running on.
3.	Compile the solution and run it on the device or emulator.

### 2) Username and Password

Android and Web Application share the same database. So, they have the same access.

Android Application provides functionalities for "Store Clerk", "Store Supervisor", "Store Manager", "Department Representative", "Employee", "Department Head” and ” Acting Department Head”.

Web Application also provides functionalities for all users.

Department Representatives may not be accurately shown below if database has been modified.

All Passwords are "Password1!". All passwords in the database are encrypted.

### Usernames

Store Manager			          - 	manager@logic.u.edu

Store Supervisor			      - 	supervisor@logic.u.edu

Store Clerk				          - 	clerk@logic.u.edu

Employee				            - 	emp@logic.u.edu

Department Head			        - 	dephead@logic.u.edu

Department Representative	  - 	deprep@logic.u.edu


#### Stationery Store
•	Clerk					            -	 clerk@logic.u.edu

•	Supervisor				        - 	supervisor@logic.u.edu

•	Manager / Department Head	-	manager@logic.u.edu

#### Accountancy Department
•	Head of Department 		    -	 dephead@logic.u.edu

•	Department Representative	-	 deprep@logic.u.edu

•	Employee 				          -	 emp@logic.u.edu

#### Business Department
•	Head of Department 		    -	dephead1@logic.u.edu

•	Department Representative	-	Donald-Tan@logic.u.edu 

•	Employee 				          -	Jerry-Coleman@logic.u.edu

#### Computer Science Department
•	Head of Department 		    -	Anna-Chan@logic.u.edu 

•	Department Representative	-	Sonic-Chan@logic.u.edu

•	Employee 				          -	Kerry-Jackman@logic.u.edu

#### Economics Department
•	Head of Department 		    -	Andrew-Ng@logic.u.edu

•	Department Representative	-	Anne-Pelosi@logic.u.edu

•	Employee 				          -	Stephen-Meyers@logic.u.edu

#### English Department
•	Head of Department 		    -	Andrew-Chan@logic.u.edu

•	Department Representative -	Anna-Summers@logic.u.edu

•	Employee                  -	Venkat-Ramanathan@logic.u.edu

#### Finance Department
•	Head of Department 		    - Anna-Hathaway@logic.u.edu

•	Department Representative	- Axel-Pompeo@logic.u.edu

•	Employee 				          - Melania-the-Cat@logic.u.edu

#### Humanities Department
•	Head of Department 		    - Andrew-Jackson@logic.u.edu

•	Department Representative	- 	deprep1@logic.u.edu

•	Employee 				          - Donald-Meyers@logic.u.edu

#### Internal Audit Department
•	Head of Department        - Anna-Stark@logic.u.edu

•	Department Representative - Axel-the-Hedgehog@logic.u.edu

•	Employee                  - Sonic-Meyers@logic.u.edu

#### Social Sciences Department
•	Head of Department        - Andrew-Lee@logic.u.edu

•	Department Representative - Anne-Meyers@logic.u.edu

•	Employee                  - Sam-Tan@logic.u.edu

### 3) System Requirements

#### Tools
•	Microsoft Visual Studio
•	Android Studio (Minimum API level 26)
•	MSSQL Server Management Studio
•	IIS Server

#### Recommended Browser 
•	Mozilla Firefox

### 4) Authors
•	Chan Yuling Elaine 
•	Cheng Zongpei 
•	Gao Jia Xue 
•	Kay Thi Swe Tun	
•	Lynn Lynn Oo 
•	Sam Jing Wen 
•	Teh Li Heng 
•	Zan Tun Khine

