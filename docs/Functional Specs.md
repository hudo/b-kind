# B-Kind functional specification

Not for profit app and website that is a platform for people to share random acts of kindness. 
Random acts of kindness can alleviate stress in the world for people. 
It can make a huge difference to people's happiness levels and sometimes restore peoples faith in humanity.
 
The site should be a social media type platform that allows people to create a profile (limited information), 
they would then be able to upload a story. It can be as small or as big as you like. I bought someone a coffee, I helped change a flat tyre or maybe even saved a person�s life.

## Actors

Actors in the system are:  
- visitor: random visitor of the website, not registered
- registered user: can thumbs up (like) stories 
- moderator: can publish or remove stories
- administrator: full access, can disable users, unpublish stories

## Publishing process

Story can be in different states:  
- draft: story visible only to author
- waiting for approval: author submitted story for approval to the moderator
- published: story visible to all website visitors
- declined: story removed from website by the moderator, visible only to author

## User Stories

### Browse stories

As a visitor, I want to browse featured stories so that I can see recent active stories  
As a visitor, I want to browse stories based on different criteria   
As a visitor, I want to read whole story

### Writing and managing a story 

As a registered user, I want to write a story so that it can be approved and published   
As a registered user, I want to edit existing story so that I can save my work and continue later  
As a registered user, I want to unpublish the story so that it's not available publicly any more  
As a moderator and administrator, I want to approve the story so that it can be published   
As a moderator and administrator, I want to unpublish (decline?) published story, so thats it's not visible to visitors any more  

### Account

As a visitor, I want to register so that I can flag story I like  
As a visitor, I want to retrieve forgotten password so that I can login with my account  
As a administrator, I want to promote registered user to moderator, so that it can have permissions to manage stories  
As a administrator, I want to ban a registered user, so that it can not login into website any more





