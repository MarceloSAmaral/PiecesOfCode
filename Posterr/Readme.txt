Marcelo Amaral

Instructions
1 - Provide an instance of SQL Server 2019
2 - Build and publish the Posterr.MSSQLDB. This can be done through Visual Studio.
3 - Edit the appsettings.json file in the Posterr.WebAPI project. Set the ConnectionString to the server and database you created.
4 - OK. The project is ready to run and use.
5 - Unit tests do not require a database.


Planning
The feature of mentioning other users by the "@" character would still not allow us to point to which of that user's posts is being replied to.
We would need to display a list of the mentioned user's posts to correctly address the reply feature.
An alternative is to treat "Mention" and "Reply" as two different features.
Mention would require the author to enter the other user via the @UserName tag.
The reply could be achieved by an action in the interface that allows the user to write a reply in such a way that he can directly relate the post and the reply.

Questions:
- Would users mention themselves?
- Would it be possible to mention more than one user in a post?
- An intense conversation can lead to a “tree” of posts, replies, and lone posts. Would it be interesting to consider creating a "thread" after a "root" post?

We would need to add another "Type of Post" to the code, as well as alter some functionalities to properly support this feature.
It would be necessary to add a "Guid? ReplyTo" property to the "Post" document model.
It would be necessary to add a "Mentioning" property to the "Post" document model, be it a UserId or a List of Users.


Critique
This project does not have Authentication or Logging features.
This project lacks design to better handle multiple time zones.
Some parts of the code could undergo further optimization, such as using AsNoTracking in some queries that are not used in a context where the data they present is not being loaded for update. 

Regarding scaling up
This API is designed in such a way that the backend can be converted to serverless functions such as Azure Functions.
In addition, the database has a non-trivial design that reduces the costs of querying and retrieving some information, but some adjustments would be necessary to make it compatible with a distributed database in the NoSQL cloud.
In the scenario where Posterr gains a lot more users and posts, the "table" or "collection" of Posts would become a resource under pressure. 
I suggest that we consider adopting a CQRS and Queues architecture and decoupling the user input on the Posterr page from the actual recording of that entry in the posts table.
Adopting a cache containing recent posts would reduce the cost of retrieving the posts that are initially displayed on the homepage. 
