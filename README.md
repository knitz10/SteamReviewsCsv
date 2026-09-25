# SteamReviewsCsv

This tool allows for easy downloading reviews from Steam using the https://store.steampowered.com/appreviews/ endpoint. 

You can customize the Url's filters and have a custom csv output if you wish to. More on that further down.

## Usage

To run this program, open your Cmd/Terminal, because this is a console application, and run ```path/to/program/binary``` (for example ```C:/Users/{YourUsername}/Downloads/SteamReviewCsv.exe``` on Windows)

**This program supports these arguments:**

- ```--help``` - shows general help
- ```{appID/Steam URL}``` - The appID of the game/app on Steam you'd want to get the reviews of(for example: 440). You can find it in the url:<br>[https://store.steampowered.com/app/**440**/Team_Fortress_2/](https://store.steampowered.com/app/440/Team_Fortress_2/). You can also use the full url. 
- ```--use-recommended-output``` - also include the recommended (by me) output. Off by default.
- ```--additional-output``` - also include Author Records and Hardware Records separately. Off by default.
- ```--custom-output``` - when used correctly, the app will generate an additional {appId}_reviews_CustomOutput.csv, which will have whatever values you set. Comma separated(example: ID,PersonaName,ReviewText). Fields written in *italic* are fields modified/added by me to make it easier for you - the user.

  <details><summary>Available values</summary>
        <details>
        <summary>Review</summary>
            - ID - Id of the review in the csv<br>
            - RecommendationId - The unique id of the recommendation<br>
            - <i>RecommendationUrl</i> - The url(based on the id) of the recommendation<br>
            - Language - language the user indicated when authoring the review<br>
            - ReviewText - text of written review<br>
            <!-- - TimestampCreated - date the review was created (unix timestamp)<br> -->
            - <i>DateCreated</i> - date the review was created<br>
            <!-- - TimestampUpdated - date the review was last updated (unix timestamp)<br> -->
            - <i>DateUpdated</i> - date the review was last updated<br>
            - VotedUp - <b>true</b> means it was a positive recommendation<br>
            - <i>Vote</i> - Vote in written text (positive/negative)<br>
            - VotesUp - the number of users that found this review helpful<br>
            - VotesFunny - the number of users that found this review funny<br>
            - WeightedVoteScore - helpfulness score<br>
            - CommentCount - number of comments posted on this review<br>
            - SteamPurchase - <b>true</b> if the user purchased the game on Steam<br>
            - ReceivedForFree - <b>true</b> if the user checked a box saying they got the app for free<br>
            - Refunded - <b>true</b> if the user refunded the app<br>
            - WrittenDuringEarlyAccess - <b>true</b> if the user posted this review while the game was in Early Access<br>
            - DeveloperResponse - text of the developer response, if any<br>
            - PrimarilySteamDeck - Did the reviewer play this game primarily on Steam Deck at the time of writing<br>
            <!-- - AppReleaseDate<br> -->
            <!-- - AppReleaseDateTime<br> -->
            - Clever - how many "Clever" awards the user received, if any<br>
            - WarmBlanket - how many "Warm Blanket" awards the user received, if any<br>
            - Saucy - how many "Saucy" awards the user received, if any<br>
            - SlowClap - how many "Slow Clap" awards the user received, if any<br>
            - TakeMyPoints - how many "Take My Points" awards the user received, if any<br>
            - Wholesome - how many "Wholesome" awards the user received, if any<br>
            - Jester - how many "Jester" awards the user received, if any<br>
            - Whoa - how many "Whoa" awards the user received, if any<br>
            - SuperStar - how many "Super Star" awards the user received, if any<br>
            - Wild - how many "Wild" awards the user received, if any<br>
            - Winner - how many "Winner" awards the user received, if any<br>
            - Beautiful - how many "Beautiful" awards the user received, if any<br>
            - Helpful - how many "Helpful" awards the user received, if any<br>
            - Fire - how many "Fire" awards the user received, if any<br>
            - Funny - how many "Funny" awards the user received, if any<br>
            - OneHundred - how many "One Hundred" awards the user received, if any<br>
            - LifeSaver - how many "Life Saver" awards the user received, if any<br>
            - Perfect - how many "Perfect" awards the user received, if any<br>
            - PlusOne - how many "Plus One" awards the user received, if any<br>
            - Smart - how many "Smart" awards the user received, if any<br>
            - PureGold - how many "Pure Gold" awards the user received, if any<br>
    </details>
    <details>
    <summary>Author</summary>
            - ReviewID - Id of the review in the csv<br>
            - SteamId - SteamID of user<br>
            - PersonaName - the current user's persona (display) name<br>
            - PersonaStatus<br>
            - <i>ProfileUrl</i> - the current user's profile url<br>
            - NumGamesOwned - number of games owned by the user<br>
            - NumReviews - number of reviews written by the user<br>
            - PlaytimeForever - lifetime playtime tracked in this app<br>
            - PlaytimeLastTwoWeeks - playtime tracked in the past two weeks for this app<br>
            - PlaytimeAtReview - playtime when the review was written<br>
            <!-- - LastPlayed<br> -->
            - LastPlayedDateTime - time for when the user last played<br>
            - <i>Avatar</i> - the current user's avatar url (low res)<br>
            - <i>FullAvatar</i> - the current user's avatar url (full res)<br>
    </details>
    <details>
    <summary>Hardware</summary>
            - Manufacturer<br>
            - Model<br>
            - DxVideoCard<br>
            - DxVendorId<br>
            - DxDeviceId<br>
            - NumGpu<br>
            - SystemRam<br>
            - Os<br>
            - CpuVendor<br>
            - CpuName<br>
            - GamingDeviceType<br>
            - DxDriverVersion<br>
            - AdapterDescription<br>
            - DriverVersion<br>
            <!-- - DriverDateRaw<br> -->
            - DriverDate<br>
            - VramSize<br>
            - ScreenWidth<br>
            - ScreenHeight<br>
            - PreciseFrameRate<br>
    </details>
</details>

- ```--custom-filters``` - when used correctly(parameter by parameter(can be null)), the app will use customized filters in the URL. Not recommended to use, and the param "all" for filter is unsupported, but if you really need to use it, here is an example: ```recent,english,positive,non_steam_purchase,10,30,2,0```. It maps to ```filter,language,review_type,purchase_type,num_per_page,day_range,start_offset,filter_offtopic_activity```. To find out more about these, look at [Steam's docs](https://partner.steamgames.com/doc/store/getreviews#:~:text=the%20parameters%20below.-,Parameters%3A,-GET%20store.steampowered).

## How to build for different systems (single file release):
<!-- Ngl, this is more for me than anyone haha -->
Windows: ```dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true```

Linux: ```dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true```

Intel Mac: ```dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true```

Apple Silicon Mac: ```dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true```
