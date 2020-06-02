# Populate Database

Run `python main.py` to import the data to your databse. Assumes that the API is listening on `http://localhost:5000`

The `sample_data.csv` file included in this directory has dummy nurses for privacy reasons, I will find a way to share the real CSV file with 269 records (including RosterOn ID) once I figure out how.

## Expected CSV format

The script assumes that each row in the CSV file is structured as follows.
    
    Last Name,First Name,RosterOn ID,Designation,Bool1,Bool2,Bool3,Bool4,Bool5,Bool6,Bool7,Bool8,Bool9,Bool10,Bool11<CR>

    Bool1 : Has ALS Skill
    Bool2 : Has Transport Skill
    Bool3 : Has Mechanical Ventilation skill
    Bool4 : Has Advanced Ventilation Skill
    Bool5 : Has Neuro Skill
    Bool6 : Has Pulmonary Catherter Skill
    Bool7 : Has Day 1 Skill
    Bool8 : Has Day 0 Skill
    Bool9 : Has Pacing Skill
    Bool10 : Has CCRT Skill
    Bool11 : Has IABP Skill

The excel sheet provided by RMH is already closesly structured to this data format, A simple export to CSV format will get the desired CSV file. The parser also assumes that *there are no heading* in the CSV file.

## Dependencies

The importer depends upon `requests` library. It can be installed using

`pip install requests`

## Python Version

Tested on python `2.7.10`


## Troubleshooting

Having troubles with your database or dotnet/ef yelling at you. If nothing else works try dropping your database and migrations and do them all over again.

    $ cd path/to/dotnet-server/HospitalAllocation

    $ rm -r Migrations

    $ rm -r bin/

    $ dotnet ef database drop

    $ dotnet ef migrations add InitialMigration

    $ dotnet ef database update
