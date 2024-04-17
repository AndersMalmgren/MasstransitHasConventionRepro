Out of the box the repro project is configured for MassTransit.EntityFrameworkCore Version 8.2.1. Please run the console program. When its done check the database.
The state will read {"Count":1} even though 3 events was triggered. 

Edit csproj and change to MassTransit.EntityFrameworkCore Version 8.1.3, makesure to restore packages. Run the program and take notice of version output. Should read 8.1.3. Now check the data base and it will correctly read {"Count":3}
