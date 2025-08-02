"start" will make the app use the "DEVELOPMENT" environment,
while anything else will make it use the "TEST" environment.

*This means that if you start the app with start for manually testing it,
and then switch try to test it, you will use the "DEVELOPMENT" environment
instead of the "TEST" environment, which means that you should probably
restart if you want to test the app automatically after manually testing it.


COMMANDS:


start - will start the backend (if ir isn't already running) and then the frontend (if it isn't already running)

*the commands below will automatically do what start before doing something else

test - will start an integration test of the entire app

activitycategory - will start a functionality test for activity categories (adding, editing, deleting them)