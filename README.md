# MockSamples
Notebook with useful mocks that can be reused in variety of .NET projects.

Available mocks:

1. Session object in MVC application

1. DB context in db client applcation using EF6

1. ...

1. ...

1. ...

##Interesting things learned during implementation:

1. Difference between IEnumerable and IQueryable:

Both will give you deferred execution.

The difference is that IQueryable<T> is the interface that allows LINQ-to-SQL (LINQ.-to-anything really) to work. So if you further refine your query on an IQueryable<T>, that query will be executed in the database, if possible.

For the IEnumerable<T> case, it will be LINQ-to-object, meaning that all objects matching the original query will have to be loaded into memory from the database.

This is quite an important difference as by working with IQueryable loading redundant rows from DB is avoided.

Source: http://stackoverflow.com/questions/2876616/returning-ienumerablet-vs-iqueryablet