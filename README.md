# Web Change Alarm
Small console application capable of checking whether website with articles has changed from the last run.

## Features ##
* Optimized for News web page with articles.
* Single run console application.
* Capable of comparing published articles from the last run.
* Capable of sending notification emails to provided recepients.
* Creates locally text these files:
  * Published articles metadata - title, published date and time.
  * Last check timestamp.
* Configurable via **config.ini** file.
* Parses emails of recipients from local text file or from hidden URL.

## CONFIG file ##
The configuration file is required. 
It currently supports there parametrs:
* [Website]
  * *homeurl* - URL of the website where you collect recipients emails.
  * *url* - URL of the website under alarm, with published articles. 
  * *nodeTitle* - Html node with published article's title, see [Documentation](https://html-agility-pack.net/select-nodes)
  * *nodePublishedAt* - Html node with published article's date and time, see [Documentation](https://html-agility-pack.net/select-nodes)
  * *nodeMessageBody* - Html node with published article's message body, see [Documentation](https://html-agility-pack.net/select-nodes)
* [Email]
  * *adminEmail* - Admin's email is added into Bcc of each send email.
  * *recipentsSourceType* - Use "url" for website or anything for text file.
  * *recipentsSource* - URL (or textfile path) with list of recipients emails separated with '\n' character.
  * *fromName* - Name that will be filled in emails as FROM.
  * *fromEmail* - Email address account from where all email notifiation will be send.
* [SmtpClient] - Parameters of email account from where all email notifiation will be send.
  * *host* - Host of email 
  * *port* - Usually "465".
  * *usessl* - Recomended to use "true".
  * *username* - Credentials for email account.
  * *password* - Credentials for email account.

### Example ###


## How to use ##



## Build on ##
These application uses [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/), 
[MailKit](https://www.nuget.org/packages/MailKit/) and [Microsoft.Extensions.Configuration.Ini](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Ini) packages.
**Thank you authors for being awesome!**