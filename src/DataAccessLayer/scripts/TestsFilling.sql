INSERT [dbo].[GrammarTests] ([id], [Name], [Description], [CountQuestions]) VALUES
	(1, N'Articles', N'Тест на употребление артиклей. Из предложенных вариантов ответа необходимо выбрать только один, который на ваш взгляд является правильным.', 10),
	(2, N'Present Tenses', N'Данный тест проверяет умение правильно выбирать и употреблять настоящие времена (Present Simple, Present Continuous, Present Perfect, Present Perfect Continuous) в соответствии с ситуацией общения. Из предложенных вариантов ответа необходимо выбрать только один, который на ваш взгляд является правильным.', 15),
	(3, N'Passive Voice', N'Тест на употребление страдательного залога в английском языке. Из предложенных вариантов ответа необходимо выбрать только один, который на ваш взгляд является правильным.', 15),
	(4, N'Past Tenses', N'Данный тест проверяет умение правильно выбирать и употреблять прошедшие времена (Past Simple, Past Continuous, Past Perfect, Past Perfect Continuous) в соответствии с ситуацией общения. Из предложенных вариантов ответа необходимо выбрать только один, который на ваш взгляд является правильным.', 15)


INSERT [dbo].[TheoryLink] ([Id], [Name], [Url], [GrammarTestId]) VALUES 
	(1, N'Артикли в английском языке', N'https://www.native-english.ru/grammar/english-articles', 1),
	(2, N'Определенный артикль в английском языке', N'https://www.native-english.ru/grammar/definite-article', 1),
	(3, N'Оеопределенный артикль в английском языке', N'https://www.native-english.ru/grammar/indefinite-article', 1),
	(4, N'Отсутствие артикля (нулевой артикль)', N'https://www.native-english.ru/grammar/zero-article', 1),
	(5, N'Present Simple - простое настоящее время', N'https://www.native-english.ru/grammar/present-simple', 2),
	(6, N'Present Continuous - настоящее длительное время', N'https://www.native-english.ru/grammar/present-continuous', 2),
	(7, N'Present Perfect - настоящее совершенное время', N'https://www.native-english.ru/grammar/present-perfect', 2),
	(8, N'Present Perfect Continuous - настоящее совершенное длительное время', N'https://www.native-english.ru/grammar/present-perfect-continuous', 2),
	(9, N'Пассивный залог (passive voice)', N'https://www.native-english.ru/grammar/passive-voice', 3),
	(10, N'Past Simple - простое прошедшее время', N'https://www.native-english.ru/grammar/past-simple', 4),
	(11, N'Past Continuous - прошедшее длительное время', N'https://www.native-english.ru/grammar/past-continuous', 4),
	(12, N'Past Perfect - прошедшее совершенное время', N'https://www.native-english.ru/grammar/past-perfect', 4),
	(13, N'Past Perfect Continuous - прошедшее совершенное длительное время', N'https://www.native-english.ru/grammar/past-perfect-continuous', 4)

INSERT [dbo].[TestQuestions] ([Id], [Text], [AnswerOptions], [GrammarTestId]) VALUES 
	(1, N'I only want {0} little sugar in my tea, please.', N'["a", "such", "the"]', 1),
	(2, N'It is important sometimes to stop and look around you at all the wonderful things {0}.', N'["in nature", "in the nature", "nature"]', 1),
	(3, N'Give me {0}, please.', N'["an apple", "the apple", "apple"]', 1),
	(4, N'What {0} girl!', N'["a strange", "the strange", "strange"]', 1),
	(5, N'Could you give me {0} of paper?', N'["a sheet", "sheet", "the sheet"]', 1),
	(6, N'This is {0} wine I have ever drunk.', N'["the best", "best", "a best"]', 1),
	(7, N'{0} Pacific Ocean is the largest ocean on {1} Earth.', N'["The,the", "A,a", "---,---"]', 1),
	(8, N'{0} had never possessed a standing army or a police force.', N'["The Tudors", "A Tudors", "Tudors"]', 1),
	(9, N'Can you give me {0} over there?', N'["the book", "book", "a book"]', 1),
	(10, N'Could you close {0}, please?', N'["the door", "door", "a door"]', 1),
	(11, N'I want to go to the cinema to see a film about {0} and the French.', N'["France", "the France", "a France"]', 1),
	(12, N'The interesting thing about {0} is all the roads that they built in Britain.', N'["the Romans", "Romans", "a Romans"]', 1),
	(13, N'Are you studying foreign languages at school, like {0}?', N'["French", "a French", "the French"]', 1),
	(14, N'Is there {0} in the street?', N'["a school", "the school", "school"]', 1),
	(15, N'Russian people like {0}.', N'["tea", "the tea", "a tea"]', 1),
	(16, N'We celebrate New Year on {0} of December.', N'["the 31st", "a 31st", "31st"]', 1),
	(17, N'{0} Earth is millions of kilometres from {1} Sun.', N'["The,the", "---,---", "A,a"]', 1),
	(18, N'{0} have left London.', N'["The Browns", "Browns", "A Browns"]', 1),
	(19, N'Great Britain consists of {0} parts.', N'["three", "the three", "a three"]', 1),
	(20, N'Pushkin, the great Russian poet, was born in {0}.', N'["1799", "the 1799", "a 1799"]', 1),
	(21, N'I am tired. We {0} for more than an hour. Let is stop and rest for a while.', N'["have been walking", "are walking", "have walked", "walk"]', 2),
	--(22, N'How long {0} a course of lectures on Medieval History?', N'["has Professor Donaldson been delivering", "is Professor Donaldson delivering", "does Professor Donaldson deliver", "has Professor Donaldson delivered"]', 2),
	(23, N'Sonia {0} as a computer programmer this year, but she would like to try something different in the future.', N'["is working", "works", "has been working", "has worked"]', 2),
	(24, N'Peter and Mary {0} on the platform. They have been waiting for their train for half an hour.', N'["are standing", "have stood", "stand", "have been standing"]', 2),
	(25, N'Be careful with paint. It {0} a certain amount of lead.', N'["contains", "is containing", "contained", "has contained"]', 2),
	(26, N'Her family {0} from town to town ever since she can remember.', N'["has been moving", "has moved", "is moving", "moves"]', 2),
	(27, N'- Hello! May I speak to John, please? - Sorry, he is out. He has gone to the library. He {0} for his History exam there.', N'["is reading", "has been reading", "reads", "has read"]', 2),
	(28, N'- Do you remember me? - Of course, I do. We {0} several times before.', N'["have met", "met", "have been meeting", "meet"]', 2),
	(29, N'Sara, my next door neighbour, has a car, but she {0} it very often.', N'["does not use", "has not used", "has not been using", "is not using"]', 2),
	(30, N'The government is worried because the number of people without jobs {0}.', N'["is increasing", "increases", "has increased", "has been increasing"]', 2),
	(31, N'Paul looks young for his age. He says he is 56 years old, but nobody {0} him.', N'["believes", "believe", "has not believed", "is not believing"]', 2),
	(32, N'Jack Strom has been a postman all his life; he {0} mail to homes and offices to the people of the town.', N'["delivers", "has been delivering", "has delivered", "is delivering"]', 2),
	(33, N'Susan is a fashion designer. Now, she {0} at a new set of clothes to be shown at a fashion show in April.', N'["is working", "has been working", "works", "has worked"]', 2),
	(34, N'Who has taken my newspaper? It was on my desk a minute ago.', N'["has taken", "takes", "have taken", "took"]', 2),
	(35, N'My cousin Jake has got a lot of books, most of which he {0}.', N'["has not read", "has not been reading", "is not reading", "does not read"]', 2),
	(36, N'In {0} end we decided not to go to the cinema but to watch television.', N'["the", "an", "this"]', 1),
	(37, N'She always said that when she grew up she wanted to be {0} .', N'["a doctor", "the doctor", "doctor"]', 1),
	(38, N'{0} drive on the left-hand side in their country.', N'["The British", "British", "A British"]', 1),
	(39, N'She is very good at {0}.', N'["painting", "a painting", "the painting"]', 1),
	(40, N'It is {0} book that I have ever read.', N'["the funniest", "a funniest", "funniest"]', 1),
	(41, N'I have left my book in {0} and I would like you to get it for me.', N'["the kitchen", "a kitchen", "kitchen"]', 1),
	(42, N'The Queen of Great Britain is not {0}.', N'["absolute", "an absolute", "the absolute"]', 1),
	(43, N'Can anyone give me {0} please because I have just fallen over?', N'["a hand", "the hand", "hand"]', 1),
	(44, N'My friend likes to eat {0}.', N'["fish", "a fish", "the fish"]', 1),
	(45, N'There are {0} and toys on the floor.', N'["books", "the books", "a books"]', 1),
	(46, N'You do not hear what I am saying because you {0} very absent-minded today.', N'["are being", "are", "have been", "is"]', 2),
	(47, N'I don not like Alice. She {0} about difficulties of life all the time.', N'["complains", "has been complaining", "has complained", "complaining"]', 2),
	(48, N'Their car is as good as new though they {0} it for a number of years.', N'["have had", "have been having", "have", "are having"]', 2),
	(49, N'I {0} Mario for some time since he left Milan a few years ago.', N'["have not seen", "do not see", "did not see", "are not seeing"]', 2),
	(50, N'I think you are being very weak. Do not get out of bed. You will only make your temperature go up again.', N'["are being", "are", "were", "have been"]', 2),
	(51, N'I have read this chapter in my chemistry text three times, and still I {0} it.', N'["don not understand", "have understood", "understand", "have not understood"]', 2),
	(52, N'How long {0} Jerry? - But I do not know him at all. I have never met him.', N'["have you known", "has you known", "do you know", "did you know"]', 2),
	(53, N'Zeta has sent me two letters; neither of which {0}.', N'["has arrived", "is arriving", "arrive", "have arrived"]', 2),
	(54, N'A group of scientists are travelling around Africa. How many countries {0} so far, I wonder?', N'["have they visited", "do they visit", "they have visited", "have they been visiting"]', 2),
	(55, N'Willy {0} from his Uncle Alex since the latter immigrated to Canada.', N'["has not heard", "is not hearing", "does not hear", "have not heard"]', 2),
	(56, N'Jerry promised to come to work in time. He is not here, and he even {0} .', N'["has not called", "is not calling", "does not call", "has not been calling"]', 2),
	(57, N'As far as I know Mike {0} Italian for quite some time, but he still does not understand very much.', N'["has been learning", "learns", "is learning", "has learnt"]', 2),
	(58, N'Look here! I simply refuse to believe what you {0} me now.', N'["are telling", "tell", "have been telling", "have told"]', 2),
	(59, N'About 85 percent of American students {0} public schools, which are supported by state and local taxes.', N'["attend", "have attended", "are attending", "have been attending"]', 2),
	(60, N'For many years American schools {0} federal aid for special purposes.', N'["have been receiving", "receive", "have received", "are receiving"]', 2),
	(61, N'Detroit {0} as the first capital city of Michigan, but now Lansing is the capital city of Michigan.', N'["was chosen", "chosen", "have been chosen"]', 3),
	(62, N'The five great lakes of the world {0} in Michigan.', N'["can be found", "can found", "can find"]', 3),
	(63, N'Battle Creek is a hard-working city, where businesses {0} dedicated employees who want to build a good life for their families.', N'["found", "have not found", "have found"]', 3),
	(64, N'Local police {0} the bank robber.', N'["have arrested", "have been arrested", "was arrested"]', 3),
	(65, N'I {0} last Friday.', N'["arrived", "was arrived", "have arrived"]', 3),
	(66, N'Tom {0} his key.', N'["has lost", "has been lost", "was lost"]', 3),
	(67, N'Many accidents {0} by dangerous driving.', N'["are caused", "have been caused", "caused"]', 3),
	(68, N'A cinema is a place where films {0}.', N'["are shown", "show", "have been shown"]', 3),
	(69, N'This house {0} in 1930.', N'["was built", "has built", "built"]', 3),
	(70, N'A new supermarket {0} next year.', N'["will be built", "is building", "will built"]', 3),
	(71, N'Two men tried to sell a painting that {0}', N'["had been stolen", "was stolen", "had stolen"]', 3),
	(72, N'They {0} this clock now.', N'["are repairing", "are being repaired", "repair"]', 3),
	(73, N'Football {0} for hundred of years.', N'["has been played", "has played", "was played"]', 3),
	(74, N'America''s first college, Harvard, {0} in Massachusetts in 1636.', N'["was founded", "had been founded", "is being founded"]', 3),
	(75, N'Everybody {0} by the terrible news yesterday.', N'["was shocked", "is shocking", "shocked"]', 3),
	(76, N'Mr. Green {0} at the University since 1989.', N'["has been teaching", "has been taught", "is teaching"]', 3),
	(77, N'The secretary {0} to her new boss yesterday.', N'["was introduced", "introduced", "is introduced"]', 3),
	(78, N'A prize {0} to whoever solves this equation.', N'["will be given", "gives", "will be giving"]', 3),
	(79, N'A dog {0} by the small red car.', N'["was hit", "was hitting", "is hitting"]', 3),
	(80, N'Detroit {0} Motown in the past.', N'["was called", "is called", "called"]', 3),
	(81, N'The book {0} by Hardy.', N'["was written", "wrote", "was wrote"]', 3),
	(82, N'A famous architect {0} the bridge.', N'["built", "was built", "have built"]', 3),
	(83, N'It''s a big company. It {0} two hundred people.', N'["employs", "employing", "is employed"]', 3),
	(84, N'People {0} this road very often.', N'["don''t use", "haven''t used", "aren''t used"]', 3),
	(85, N'Have you heard the news? The President {0}!', N'["has been shot", "has shot", "shot"]', 3),
	(86, N'I don''t think we must {0} everything tomorrow.', N'["finish", "have finished", "be finished"]', 3),
	(87, N'This is a large hall. Many parties {0} here.', N'["are held", "are being held", "has been held"]', 3),
	(88, N'The Earth''s surface {0} mostly {1} with water.', N'["is,covered", "has,been covered", "was,covered"]', 3),
	(89, N'Not much {0} about the accident since that time.', N'["has been said", "said", "has said"]', 3),
	(90, N'The university of Michigan is one of the best universities in the United States and it {0} in Ann Arbor.', N'["is located", "location", "located"]', 3),
	(91, N'I didn''t see Linda last month because she {0} around Europe at that time.', N'["was travelling", "had travelled", "had been travelling", "travelled"]', 4),
	(92, N'Margaret didn''t wear her shoes; she was barefoot. She {0} on a piece of broken glass and cut her foot.', N'["stepped", "was stepping", "had been stepping", "had stepped"]', 4),
	(93, N'Sam says he didn''t enjoy the program because the TV set {0} properly.', N'["wasn''t working", "hadn''t been working", "hadn''t worked", "didn''t work"]', 4),
	(94, N'Before I went to bed I decided to check the front door. I was sure my sister {0} it. And I was right!', N'["hadn''t locked", "had locked", "didn''t lock", "locked"]', 4),
	(95, N'Robert didn''t answer the phone when Mary called. He {0} a shower and didn''t hear the phone ring.', N'["was taking", "took", "had been taking", "had taken"]', 4),
	(96, N'He didn''t see me as he was reading when I {0} into the room.', N'["came", "had come", "was coming", "had been coming"]', 4),
	(97, N'Before Adam got married, he {0} hiking to the mountains every summer. Now he goes to the seaside with his wife.', N'["would go", "had been going", "had gone", "went"]', 4),
	(98, N'Yesterday I came up to a stranger who looked like Jane Faster and started talking to her. But she wasn''t Jane. It was clear I {0} a mistake.', N'["had made", "made", "had been making", "was making"]', 4),
	(99, N'He was taken to the police station because he {0} into a car in front of him.', N'["had crashed", "didn''t crash", "crashed", "wasn''t crashing"]', 4),
	(100, N'When I first {0} to England in 1938, I thought I knew English fairly well.', N'["came", "had been coming", "was coming", "had come"]', 4),
	(101, N'A man once built a house and {0} his friends to visit him.', N'["invited", "had been inviting", "was inviting", "had invited"]', 4),
	(102, N'Alan {0} out almost every day last year, but now he can''t afford it.', N'["used to eat", "had eaten", "ate", "was eating"]', 4),
	(103, N'He had been away for many years and when he visited his native town, he saw that it {0} greatly.', N'["had changed", "was changing", "changed", "had been changing"]', 4),
	(104, N'When I was young, I {0} that people over forty were very old. Now that I am forty myself I don''t think so.', N'["used to think", "had thought", "was thinking", "thought"]', 4),
	(105, N'When Alice was small, she {0} of darkness and always slept with the light on.', N'["was afraid", "had been afraid", "afraided", "used to be afraid"]', 4),
	(106, N'In 1912 the Titanic {0} an iceberg on its first trip across the Atlantic, and it sank four hours later.', N'["hit", "had hit", "was hitting", "had been hitting"]', 4),
	(107, N'Tom {0} breakfast this morning because he didn''t have any time', N'["didn''t eat", "wasn''t eating", "hadn''t eaten", "hadn''t been eating"]', 4),
	(108, N'While I {0} a burglar climbed into the room through the window.', N'["was sleeping", "had been sleeping", "had slept", "slept"]', 4),
	(109, N'While the kids {0} in the garden, their mother was hurriedly cooking dinner.', N'["were playing", "played", "had been playing", "had played"]', 4),
	(110, N'Scarcely {0} out of the window when I saw a flash of light.', N'["had I looked", "I was looking", "was I looking", "had I been looking"]', 4),
	(111, N'A strong wind {0} and I decided to put on a warm coat.', N'["was blowing", "had been blowing", "had blown", "blew"]', 4),
	(112, N'When Mary came back, she looked very red from the sun. She {0} in the sun too long.', N'["had been lying", "lay", "was lying", "had lain"]', 4),
	(113, N'Everybody was laughing merrily while Harris {0} them a funny story.', N'["was telling", "had been telling", "had told", "told"]', 4),
	(114, N'Who {0} in this house before they pulled it down?', N'["had been living", "lived", "had lived", "was living"]', 4),
	(115, N'While I {0} the dishes last night, I dropped a plate and broke it.', N'["was washing", "had washed", "washed", "had been washing"]', 4),
	(116, N'I looked everywhere for my car keys and then I remembered that my son {0} the car to work.', N'["had taken", "was taking", "had been taking", "took"]', 4),
	(117, N'The trouble started when Mrs. Leslie Cady {0} control of her car on a narrow mountain road.', N'["had lost", "was losing", "had been losing", "lost"]', 4),
	(118, N'I handed Betsy today''s newspaper, but she didn''t want it. She {0} it during her lunch.', N'["had read", "read", "had been reading", "was reading"]', 4),
	(119, N'The Browns {0} in a large house when their children were at home, but they moved to a small three-room apartment after the children grew up and left home.', N'["used to live", "were living", "lived", "had lived"]', 4),
	(120, N'Rescue workers {0} a man, a woman, and two children from cold rushing water.', N'["pulled", "had pulled", "were pulling", "had been pulling"]', 4)