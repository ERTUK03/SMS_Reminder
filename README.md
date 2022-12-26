# SMS_Reminder

Just a simple project meant to be deployed to Azure that that mimics functionality
of largely used SMS reminders. It works by connecting to a database where is a table
that contains data such as name, lastname, phone and date. Then it pulls records from
that database and if the date specified is exactly 1 day away from current date then 
it sends a message with some text.

To work with Azure it needs Docker.
SMS sending works thanks to [Twilio](https://www.twilio.com/).
