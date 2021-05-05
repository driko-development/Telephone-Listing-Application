# Telephone-Listing-Application
---
## Goal
Produce a command-line driven telephone listing program. The program shall be capable of receiving and storing a list of people with their full name and telephone. <br>

## Description of how the code works
The code starts by doing input validation on the command line arguments. It is during this initial set up that the regular expressions are used to validate the input. <br>
After successful set up, then using the TelephoneListingService the specified command is run using its corresponding parameters. <br>
The ListingService utilizes a SQLite Person database with Person table to keep track of people in the telephone list.

## Commands
* ADD <Person\> - Add a new person to the database
* DEL <Person\> - Remove someone by name
* DEL <Telephone #> - Remove someone by Telephone number
* LIST - Produce a list of the members of the database

# Prerequisites
---
## Installing .NET on Linux
This project is tested on [Ubuntu 20.04 LTS](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu), but other linux distros are supported. <br>

See the Microsoft documentation for [Linux Distributions](https://docs.microsoft.com/en-us/dotnet/core/install/linux).

## Installing and Setting up SQLite
See the following tutorial on how to [install SQLite on Ubuntu](https://linuxhint.com/install_sqlite_browser_ubuntu_1804/). <br>

Run the following commands inside the project folder "TelephoneListing" to create the Person database table <br>
>sqlite3 person.db3
>
>sqlite> CREATE TABLE Person(
>  Name VARCHAR(255) NOT NULL,
>  PhoneNumber VARCHAR(255) NOT NULL  
>);
>

# Compilation and Execution
---
1.  cd into the Telephone-Listing directory
2.  dotnet run - to compile and run the program
3.  read the program usage and run the commands <br>
**Note**: The dotnet CLI will not work unless inside the CSProject root folder.

# Assumptions made
---
1.  The user is able to set up the SQLite Person database with Person table
2.  Assuming the user creates the SQLite database in the root folder of the CSProject
3.  Assumes the user changes the permissions and ownership of the log file and configuration file

# Pros / Cons
---
## Pros
*   Utilizes Dependency Injection for configuration files and logging functionality.
*   SQL injection protection via parameterized SQL queries

## Cons
*   Not much exception handling especially when dealing with the SQL statements, due to time limitation.
*   Logging template does not contain real user id
*   Due to dotnet not creating an executeable hard to change ownership and permissions
