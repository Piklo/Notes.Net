CREATE TABLE Users(
    Id varchar(255) NOT NULL UNIQUE,
    Login varchar(255) NOT NULL UNIQUE,
    PasswordHash varchar(255) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE Notes(
    Id varchar(255) NOT NULL UNIQUE,
    UserId varchar(255) NOT NULL,
    Value text NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_User_Note FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE
);