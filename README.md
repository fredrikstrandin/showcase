# showcase


Personuppgifter skall sparas separat i egen databas så att tex om adressen ändras gäller ändringen samtliga ansökningar.
På grund av GDPR kan vi inte ha personnummer som nyckel då den i så fall kommer synas i länkar och i token.

Denna lösning kommer senare att byggas på där av att man bygger en microservis från början och att varje service är ansvarig för sitt eget data. 
Dock är personens inkomst en avvägning. Man kan välja att Lånetjänsten kommunicerar med Persontjänsten eller att man låter persontjänsten meddela omvärlden att ändring har ägt rum 
då att övriga tjänster får uppdatera sig. Detta skulle göra att beroendet av varandra minskar men ökar datamängden.

I detta fall är uppgiften utformad för att bygga med REST, skulle dt visa sig att det är bara ett interface som skall prata med tjänsterna kan det vara värt att titta på GraphQL 
för att på så sätt få en endpoint att kommunicera med. Det skulle även göra att man kan välja vilken data som skall hämtas. Arkitekturen skulle så se likadan ut för utom att 
GraphQL tjänsten skulle hämta sin data via gRPC direkt från de olika tjänsterna i stället för att gå via REST. Detta för att öka prestandan då denna överföring är binär. 

Vidare är ingen OAuth implementerad mest av tidsbrist. Detta göra att lösningen inte på något sätt är säker. Men det gör även att tjänsterna inte vet vem som anropar dem. 
Därav att man skickar med användarid när man tillexempel ska ändra en ansökan. Eller att alla får ta bort en ansökan.

Implementationen av gRPC borde gjorts med ett projekt till där Proto filen ligger så att man bara ändrar den på ett ställe, den måste vara den samma på server och klient.

Att välja att implementera ett tjänstelager och ett repositorylager där det kanske inte är så stort behov beror på att jag vill ha logiken i tjänstelagret och hämtningen 
i repositoryt. Detta för att det blir lättare att byta ut underliggande repository. Att byta ut databasen är i de absolut flesta fallen inte så aktuellt förutom när 
man skall testa. Då kan man tillexempel vilja att löneuppgifterna som hämtas alltid skall returnera 40.000 och man inte skall behöva hämta det från databasen. Så delar vi 
upp det blir det lättare att mocka data. Där av att id alltid är sting tills det kommer till repositoryt där man i detta fallet gör om det till Guid. Men kunde lika gärna 
varit en int som databasen returnerade.

Validering av lånet valde jag att göra med interface. Det vill säga att man skapar en klass registrerar den med interfacet. Föreden är att man får en lätt underhållen kodbas.
I .Net Core så blir det en array av de klasser man registrerar på ett interface. Detta göra att man kan loppa igenom 
de klasser som är registrerade. I fallet för att värdera en ansökan har interfacet två metoder. Första är att kontrollera om denna klass skall utvärdera denna lånetyp. Den 
andra metoden kontrollerar om ansökan uppfyller just dess vilkor. Man kan välja att avsluta så fort man fått ett avslag, jag valde dock att kolla alla vilkor så att man får 
veta alla avslagsorsaker på engång. För liten inkomst och avbetalningsperioden får inte vara längre än 10 år osv. 

