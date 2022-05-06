GO
INSERT [dbo].[GrammarTest] ([id], [Name], [Description], [CountQuestions]) VALUES (1, N'Articles', N'Тест на употребление артиклей. Из предложенных вариантов ответа необходимо выбрать только один, который на ваш взгляд является правильным.', 10)
GO
INSERT [dbo].[GrammarTest] ([id], [Name], [Description], [CountQuestions]) VALUES (2, N'Present Tenses', N'Данный тест проверяет умение правильно выбирать и употреблять настоящие времена (Present Simple, Present Continuous, Present Perfect, Present Perfect Continuous) в соответствии с ситуацией общения. Из предложенных вариантов ответа необходимо выбрать только один, который на ваш взгляд является правильным.', 15)
GO
SET IDENTITY_INSERT [dbo].[TestQuestion] ON 
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (1, N'I only want {0} little sugar in my tea, please.', N'["a", "such", "the"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (2, N'It is important sometimes to stop and look around you at all the wonderful things {0}.', N'["in nature", "in the nature", "nature"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (3, N'Give me {0}, please.', N'["an apple", "the apple", "apple"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (4, N'What {0} girl!', N'["a strange", "the strange", "strange"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (5, N'Could you give me {0} of paper?', N'["a sheet", "sheet", "the sheet"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (6, N'This is {0} wine I have ever drunk.', N'["the best", "best", "a best"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (7, N'{0} Pacific Ocean is the largest ocean on {1} Earth.', N'["The,the", "A,a", "---,---"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (8, N'{0} had never possessed a standing army or a police force.', N'["The Tudors", "A Tudors", "Tudors"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (9, N'Can you give me {0} over there?', N'["the book", "book", "a book"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (10, N'Could you close {0}, please?', N'["the door", "door", "a door"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (11, N'I want to go to the cinema to see a film about {0} and the French.', N'["France", "the France", "a France"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (12, N'The interesting thing about {0} is all the roads that they built in Britain.', N'["the Romans", "Romans", "a Romans"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (13, N'Are you studying foreign languages at school, like {0}?', N'["French", "a French", "the French"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (14, N'Is there {0} in the street?', N'["a school", "the school", "school"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (15, N'Russian people like {0}.', N'["tea", "the tea", "a tea"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (16, N'We celebrate New Year on {0} of December.', N'["the 31st", "a 31st", "31st"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (17, N'{0} Earth is millions of kilometres from {1} Sun.', N'["The,the", "---,---", "A,a"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (18, N'{0} have left London.', N'["The Browns", "Browns", "A Browns"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (19, N'Great Britain consists of {0} parts.', N'["three", "the three", "a three"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (20, N'Pushkin, the great Russian poet, was born in {0}.', N'["1799", "the 1799", "a 1799"]', 1)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (21, N'I am tired. We {0} for more than an hour. Let is stop and rest for a while.', N'["have been walking", "are walking", "have walked", "walk"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (22, N'How long {0} a course of lectures on Medieval History?', N'["has Professor Donaldson been delivering", "is Professor Donaldson delivering", "does Professor Donaldson deliver", "has Professor Donaldson delivered"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (23, N'Sonia {0} as a computer programmer this year, but she would like to try something different in the future.', N'["is working", "works", "has been working", "has worked"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (24, N'Peter and Mary {0} on the platform. They have been waiting for their train for half an hour.', N'["are standing", "have stood", "stand", "have been standing"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (25, N'Be careful with paint. It {0} a certain amount of lead.', N'["contains", "is containing", "contained", "has contained"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (26, N'Her family {0} from town to town ever since she can remember.', N'["has been moving", "has moved", "is moving", "moves"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (27, N'- Hello! May I speak to John, please? - Sorry, he is out. He has gone to the library. He {0} for his History exam there.', N'["is reading", "has been reading", "reads", "has read"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (28, N'- Do you remember me? - Of course, I do. We {0} several times before.', N'["have met", "met", "have been meeting", "meet"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (29, N'Sara, my next door neighbour, has a car, but she {0} it very often.', N'["does not use", "has not used", "has not been using", "is not using"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (30, N'The government is worried because the number of people without jobs {0}.', N'["is increasing", "increases", "has increased", "has been increasing"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (31, N'Paul looks young for his age. He says he is 56 years old, but nobody {0} him.', N'["believes", "believe", "has not believed", "is not believing"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (32, N'Jack Strom has been a postman all his life; he {0} mail to homes and offices to the people of the town.', N'["delivers", "has been delivering", "has delivered", "is delivering"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (33, N'Susan is a fashion designer. Now, she {0} at a new set of clothes to be shown at a fashion show in April.', N'["is working", "has been working", "works", "has worked"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (34, N'Who has taken my newspaper? It was on my desk a minute ago.', N'["has taken", "takes", "have taken", "took"]', 2)
GO
INSERT [dbo].[TestQuestion] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES (35, N'My cousin Jake has got a lot of books, most of which he {0}.', N'["has not read", "has not been reading", "is not reading", "does not read"]', 2)
GO
SET IDENTITY_INSERT [dbo].[TestQuestion] OFF
GO
INSERT [dbo].[TheoryLink] ([Id], [Name], [Url], [GrammarTestId]) VALUES (N'1', N'Артикли в английском языке', N'https://www.native-english.ru/grammar/english-articles', 1)
GO
INSERT [dbo].[TheoryLink] ([Id], [Name], [Url], [GrammarTestId]) VALUES (N'2', N'Определенный артикль в английском языке', N'https://www.native-english.ru/grammar/definite-article', 1)
GO
INSERT [dbo].[TheoryLink] ([Id], [Name], [Url], [GrammarTestId]) VALUES (N'3', N'NUНеопределенный артикль в английском языкеLL', N'https://www.native-english.ru/grammar/indefinite-article', 1)
GO
INSERT [dbo].[TheoryLink] ([Id], [Name], [Url], [GrammarTestId]) VALUES (N'4', N'Отсутствие артикля (нулевой артикль)', N'https://www.native-english.ru/grammar/zero-article', 1)
GO
INSERT [dbo].[TheoryLink] ([Id], [Name], [Url], [GrammarTestId]) VALUES (N'5', N'Present Simple - простое настоящее время', N'https://www.native-english.ru/grammar/present-simple', 2)
GO
INSERT [dbo].[TheoryLink] ([Id], [Name], [Url], [GrammarTestId]) VALUES (N'6', N'
Present Continuous - настоящее длительное время', N'https://www.native-english.ru/grammar/present-continuous', 2)
GO
INSERT [dbo].[TheoryLink] ([Id], [Name], [Url], [GrammarTestId]) VALUES (N'7', N'Present Perfect - настоящее совершенное время', N'https://www.native-english.ru/grammar/present-perfect', 2)
GO
INSERT [dbo].[TheoryLink] ([Id], [Name], [Url], [GrammarTestId]) VALUES (N'8', N'Present Perfect Continuous - настоящее совершенное длительное время', N'https://www.native-english.ru/grammar/present-perfect-continuous', 2)
GO
