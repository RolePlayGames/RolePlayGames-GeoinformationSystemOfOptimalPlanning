# GEOINFORMATION SYSTEM OF OPTIMAL PLANNING

---

[![Typing SVG](https://readme-typing-svg.herokuapp.com?font=Fira+Code&pause=1000&color=4F38F7&width=500&lines=Geoinformation+system+of+optimal+planning)](https://git.io/typing-svg)

---

The GEOINFORMATION SYSTEM OF OPTIMAL PLANNING does nothing for now =)

---

## Technology stack

#### Web stack:
- ReactTS - application framework
- MaterialUI - component library
- Jest with React Testing Library - testing framework
- eslint - linter framework

#### Backend stack:

- ASP.NET Core (.NET 7) - application framework
- Dapper 6 - ORM
- xUnit - testing framework
- StyleCop - linter framework

## Development

### WebAPI solution linting

To lint WebAPI solution you need to open command line in /WebAPI folder and run:
```sh
dotnet format --severity warn --verify-no-changes
```

### Database

#### Local PostgreSQL with docker

Be careful, you cannot work with docker container from another docker container without additional settings.

- Download the latest postgres image (latest tag is default):
```
docker pull postgres
```
- Run loaded image:
```sh
docker run --name postgres -p 5432:5432 -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=postgres -d postgres
```