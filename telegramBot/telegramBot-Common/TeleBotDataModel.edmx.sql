
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/27/2019 15:50:10
-- Generated from EDMX file: C:\Users\msvs\Source\Repos\telegrambot\telegramBot\telegramBot-Common\TeleBotDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TeleBotDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SentRepositoryUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SentRepositories] DROP CONSTRAINT [FK_SentRepositoryUser];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SentRepositories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SentRepositories];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int  NOT NULL,
    [gitToken] nvarchar(100)  NULL,
    [updateFrequency] int  NOT NULL,
    [lastUpdate] datetime  NOT NULL
);
GO

-- Creating table 'SentRepositories'
CREATE TABLE [dbo].[SentRepositories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SentRepositories'
ALTER TABLE [dbo].[SentRepositories]
ADD CONSTRAINT [PK_SentRepositories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'SentRepositories'
ALTER TABLE [dbo].[SentRepositories]
ADD CONSTRAINT [FK_SentRepositoryUser]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SentRepositoryUser'
CREATE INDEX [IX_FK_SentRepositoryUser]
ON [dbo].[SentRepositories]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------