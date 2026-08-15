This is **Movie suggestion** project Using :
**Backend** : ASP.NET 10 , Microsoft SQL Server 
**Frontend** : Tailwind , React 
All services are containerized using **Docker**

**HOW TO RUN**
1. Clone the repo
2. cd to project's folder
3. Run docker compose up --build (you must run docker engine before it)
4. You can find the app on (localhost:3000)

**TMDB API KEY**
You can either create account on TMDB and get API KEY and put it in appsetting.json **OR** 
just connect to sql server running inside container on port 1433 then run script.sql to insert all data into database.
