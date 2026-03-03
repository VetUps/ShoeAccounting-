-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: localhost    Database: shoes_db_2
-- ------------------------------------------------------
-- Server version	8.0.44

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `category_id` int NOT NULL AUTO_INCREMENT,
  `category_title` varchar(45) NOT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Женская обувь'),(2,'Мужская обувь');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manufacturers`
--

DROP TABLE IF EXISTS `manufacturers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufacturers` (
  `manufacturer_id` int NOT NULL AUTO_INCREMENT,
  `manufacturer_title` varchar(100) NOT NULL,
  PRIMARY KEY (`manufacturer_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufacturers`
--

LOCK TABLES `manufacturers` WRITE;
/*!40000 ALTER TABLE `manufacturers` DISABLE KEYS */;
INSERT INTO `manufacturers` VALUES (1,'Alessio Nesca'),(2,'CROSBY'),(3,'Kari'),(4,'Marco Tozzi'),(5,'Rieker'),(6,'Рос');
/*!40000 ALTER TABLE `manufacturers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order_positions`
--

DROP TABLE IF EXISTS `order_positions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order_positions` (
  `order_position_id` int NOT NULL AUTO_INCREMENT,
  `order_id` int NOT NULL,
  `product_article` char(6) NOT NULL,
  `product_quantity` int NOT NULL,
  PRIMARY KEY (`order_position_id`),
  KEY `o_p_product_id_fk_idx` (`product_article`),
  KEY `o_p_order_id_fk_idx` (`order_id`),
  CONSTRAINT `o_p_order_id_fk` FOREIGN KEY (`order_id`) REFERENCES `orders` (`order_id`),
  CONSTRAINT `o_p_product_id_fk` FOREIGN KEY (`product_article`) REFERENCES `products` (`product_article`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order_positions`
--

LOCK TABLES `order_positions` WRITE;
/*!40000 ALTER TABLE `order_positions` DISABLE KEYS */;
INSERT INTO `order_positions` VALUES (2,1,'F635R4',2),(3,2,'H782T5',1),(4,2,'G783F5',1),(5,3,'J384T6',10),(6,3,'D572U8',10),(7,4,'F572H7',5),(8,4,'D329H3',4),(10,5,'F635R4',2),(11,6,'H782T5',1),(12,6,'G783F5',1),(13,7,'J384T6',10),(14,7,'D572U8',10),(15,8,'F572H7',5),(16,8,'D329H3',4),(17,9,'B320R5',5),(18,9,'G432E4',1),(19,10,'S213E3',5),(20,10,'E482R4',5),(24,13,'D572U8',11),(31,11,'F635R4',1),(32,11,'B431R5',67),(34,15,'B320R5',6),(35,16,'D572U8',1);
/*!40000 ALTER TABLE `order_positions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `order_id` int NOT NULL AUTO_INCREMENT,
  `order_date_make` date NOT NULL,
  `order_date_receipt` date NOT NULL,
  `pick_up_point_id` int NOT NULL,
  `user_id` int NOT NULL,
  `order_receipt_code` varchar(10) DEFAULT NULL,
  `order_status` enum('Новый','Завершен') DEFAULT 'Новый',
  PRIMARY KEY (`order_id`),
  KEY `o_pick_up_id_fk_idx` (`pick_up_point_id`),
  KEY `o_user_id_idx` (`user_id`),
  CONSTRAINT `o_pick_up_id_fk` FOREIGN KEY (`pick_up_point_id`) REFERENCES `pick_up_points` (`pick_up_point_id`),
  CONSTRAINT `o_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,'2025-02-27','2025-04-20',1,10,'901','Завершен'),(2,'2022-09-28','2025-04-21',11,4,'902','Завершен'),(3,'2025-03-21','2025-04-22',2,6,'903','Завершен'),(4,'2025-02-20','2025-04-23',11,5,'904','Завершен'),(5,'2025-03-17','2025-04-24',2,10,'905','Завершен'),(6,'2025-03-01','2025-04-25',15,4,'906','Завершен'),(7,'2025-02-28','2025-04-26',3,6,'907','Завершен'),(8,'2025-03-31','2025-04-27',19,5,'908','Новый'),(9,'2025-04-02','2025-04-28',5,10,'909','Новый'),(10,'2025-04-03','2025-04-29',19,10,'910','Новый'),(11,'2026-02-18','2026-02-27',6,4,NULL,'Новый'),(13,'2026-02-18','2026-03-07',6,4,NULL,'Новый'),(15,'2026-02-19','2026-02-19',4,4,NULL,'Завершен'),(16,'2026-02-19','2026-02-19',4,4,NULL,'Новый');
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pick_up_points`
--

DROP TABLE IF EXISTS `pick_up_points`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pick_up_points` (
  `pick_up_point_id` int NOT NULL AUTO_INCREMENT,
  `pick_up_point_postal_code` char(6) NOT NULL,
  `pick_up_point_city` varchar(100) NOT NULL,
  `pick_up_point_street` varchar(100) NOT NULL,
  `pick_up_point_home` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`pick_up_point_id`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pick_up_points`
--

LOCK TABLES `pick_up_points` WRITE;
/*!40000 ALTER TABLE `pick_up_points` DISABLE KEYS */;
INSERT INTO `pick_up_points` VALUES (1,'420151','г. Лесной','ул. Вишневая','32'),(2,'125061','г. Лесной','ул. Подгорная','8'),(3,'630370','г. Лесной','ул. Шоссейная','24'),(4,'400562','г. Лесной','ул. Зеленая','32'),(5,'614510','г. Лесной','ул. Маяковского','47'),(6,'410542','г. Лесной','ул. Светлая','46'),(7,'620839','г. Лесной','ул. Цветочная','8'),(8,'443890','г. Лесной','ул. Коммунистическая','1'),(9,'603379','г. Лесной','ул. Спортивная','46'),(10,'603721','г. Лесной','ул. Гоголя','41'),(11,'410172','г. Лесной','ул. Северная','13'),(12,'614611','г. Лесной','ул. Молодежная','50'),(13,'454311','г. Лесной','ул. Новая','19'),(14,'660007','г. Лесной','ул. Октябрьская','19'),(15,'603036','г. Лесной','ул. Садовая','4'),(16,'394060','г. Лесной','ул. Фрунзе','43'),(17,'410661','г. Лесной','ул. Школьная','50'),(18,'625590','г. Лесной','ул. Коммунистическая','20'),(19,'625683','г. Лесной','ул. 8Марта',''),(20,'450983','г. Лесной','ул. Комсомольская','26'),(21,'394782','г. Лесной','ул. Чехова','3'),(22,'603002','г. Лесной','ул. Дзержинского','28'),(23,'450558','г. Лесной','ул. Набережная','30'),(24,'344288','г. Лесной','ул. Чехова','1'),(25,'614164','г. Лесной',' ул. Степная','30'),(26,'394242','г. Лесной','ул. Коммунистическая','43'),(27,'660540','г. Лесной','ул. Солнечная','25'),(28,'125837','г. Лесной','ул. Шоссейная','40'),(29,'125703','г. Лесной','ул. Партизанская','49'),(30,'625283','г. Лесной','ул. Победы','46'),(31,'614753','г. Лесной','ул. Полевая','35'),(32,'426030','г. Лесной','ул. Маяковского','44'),(33,'450375','г. Лесной','ул. Клубная','44'),(34,'625560','г. Лесной','ул. Некрасова','12'),(35,'630201','г. Лесной','ул. Комсомольская','17'),(36,'190949','г. Лесной','ул. Мичурина','26');
/*!40000 ALTER TABLE `pick_up_points` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `product_article` char(6) NOT NULL,
  `product_title` varchar(100) NOT NULL,
  `product_unit` varchar(10) NOT NULL,
  `product_price` decimal(12,2) NOT NULL,
  `provider_id` int DEFAULT NULL,
  `manufacturer_id` int DEFAULT NULL,
  `category_id` int DEFAULT NULL,
  `product_discount` double DEFAULT '0',
  `product_quantity_in_stock` int NOT NULL,
  `product_description` text,
  `product_photo` mediumblob,
  PRIMARY KEY (`product_article`),
  KEY `p_provierd_id_fk_idx` (`provider_id`),
  KEY `p_manufacturer_id_fk_idx` (`manufacturer_id`),
  KEY `p_category_id_fk_idx` (`provider_id`),
  KEY `p_category_id_fk` (`category_id`),
  CONSTRAINT `p_category_id_fk` FOREIGN KEY (`category_id`) REFERENCES `categories` (`category_id`),
  CONSTRAINT `p_manufacturer_id_fk` FOREIGN KEY (`manufacturer_id`) REFERENCES `manufacturers` (`manufacturer_id`),
  CONSTRAINT `p_provierd_id_fk` FOREIGN KEY (`provider_id`) REFERENCES `providers` (`provider_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES ('AHTY4K','Ботинки','шт.',2700.00,2,5,2,2,0,NULL,NULL),('B320R5','Туфли123','шт.',21233.00,1,5,1,50,6,'Туфли Rieker женские демисезонные, размер 41, цвет коричневый',_binary '...'),('B431R5','Ботинки','шт.',2700.00,2,5,2,2,5,'Мужские кожаные ботинки/мужские ботинки',_binary '...'),('C436G5','Ботинки','шт.',10200.00,1,1,1,15,9,'Ботинки женские, ARGO, размер 40',_binary '...'),('D268G5','Туфли','шт.',4399.00,2,5,1,3,12,'Туфли Rieker женские демисезонные, размер 36, цвет коричневый',_binary '...'),('D329H3','Полуботинки','шт.',1890.00,2,1,1,4,4,'Полуботинки Alessio Nesca женские 3-30797-47, размер 37, цвет: бордовый',_binary '...'),('D364R4','Туфли','шт.',12400.00,1,3,1,16,0,'Туфли Luiza Belly женские Kate-lazo черные из натуральной замши',_binary '...'),('D572U8','Кроссовки','шт.',4100.00,2,6,2,3,6,'129615-4 Кроссовки мужские',_binary '...'),('E482R4','Полуботинки','шт.',1800.00,1,3,1,2,14,'Полуботинки kari женские MYZ20S-149, размер 41, цвет: черный',_binary '...'),('F427R5','Ботинки','шт.',11800.00,2,5,1,15,11,'Ботинки на молнии с декоративной пряжкой FRAU',_binary '...'),('F572H7','Туфли','шт.',2700.00,1,4,1,2,14,'Туфли Marco Tozzi женские летние, размер 39, цвет черный',_binary '...'),('F635R4','Ботинки','шт.',3244.00,2,4,1,2,13,'Ботинки Marco Tozzi женские демисезонные, размер 39, цвет бежевый',_binary '...'),('G432E4','Туфли','шт.',2800.00,1,3,1,3,15,'Туфли kari женские TR-YR-413017, размер 37, цвет: черный',_binary '...'),('G531F4','Ботинки','шт.',6600.00,1,3,1,12,9,'Ботинки женские зимние ROMER арт. 893167-01 Черный',_binary '...'),('G783F5','Ботинки','шт.',5900.00,1,6,2,2,8,'Мужские ботинки Рос-Обувь кожаные с натуральным мехом',_binary '...'),('H535R5','Ботинки','шт.',2300.00,2,5,1,2,7,'Женские Ботинки демисезонные',_binary '...'),('H782T5','Туфли','шт.',4499.00,1,3,2,4,5,'Туфли kari мужские классика MYZ21AW-450A, размер 43, цвет: черный',_binary '...'),('J384T6','Ботинки','шт.',3800.00,2,5,2,2,16,'B3430/14 Полуботинки мужские Rieker',_binary '...'),('J542F5','Тапочки','шт.',500.00,1,3,2,13,0,'Тапочки мужские Арт.70701-55-67син р.41',_binary '...'),('K345R4','Полуботинки','шт.',2100.00,2,2,2,2,3,'407700/01-02 Полуботинки мужские CROSBY',_binary '...'),('K358H6','Тапочки','шт.',599.00,1,5,2,20,2,'Тапочки мужские син р.41',_binary '...'),('L754R4','Полуботинки','шт.',1700.00,1,3,1,2,7,'Полуботинки kari женские WB2020SS-26, размер 38, цвет: черный',_binary '...'),('M542T5','Кроссовки','шт.',2800.00,2,5,2,18,3,'Кроссовки мужские TOFA',_binary '...'),('N457T5','Полуботинки','шт.',4600.00,1,2,1,3,13,'Полуботинки Ботинки черные зимние, мех',_binary '...'),('O754F4','Туфли','шт.',5400.00,2,5,1,4,18,'Туфли женские демисезонные Rieker артикул 55073-68/37',_binary '...'),('P764G4','Туфли','шт.',6800.00,1,2,1,15,15,'Туфли женские, ARGO, размер 38',_binary '...'),('PZ80ZM','123','шт.',12311.00,1,3,2,1,1,NULL,NULL),('S213E3','Полуботинки','шт.',2156.00,2,2,2,3,6,'407700/01-01 Полуботинки мужские CROSBY',_binary '...'),('S326R5','Тапочки','шт.',9900.00,2,2,2,17,15,'Мужские кожаные тапочки \"Профиль С.Дали\" ',_binary '...'),('S634B5','Кеды','шт.',5500.00,2,2,2,3,0,'Кеды Caprice мужские демисезонные, размер 42, цвет черный',_binary '...');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `providers`
--

DROP TABLE IF EXISTS `providers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `providers` (
  `provider_id` int NOT NULL AUTO_INCREMENT,
  `provider_title` varchar(100) NOT NULL,
  PRIMARY KEY (`provider_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `providers`
--

LOCK TABLES `providers` WRITE;
/*!40000 ALTER TABLE `providers` DISABLE KEYS */;
INSERT INTO `providers` VALUES (1,'Kari'),(2,'Обувь для вас');
/*!40000 ALTER TABLE `providers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `user_role` enum('Авторизированный клиент','Менеджер','Администратор') DEFAULT 'Авторизированный клиент',
  `user_firstname` varchar(60) NOT NULL,
  `user_lastname` varchar(60) NOT NULL,
  `user_patronymic` varchar(60) DEFAULT NULL,
  `user_login` varchar(255) NOT NULL,
  `user_password` varchar(25) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Менеджер','Ворсин','Петр','Евгеньевич','tjde7c@yahoo.com','YOyhfR'),(2,'Авторизированный клиент','Ворсин','Петр','Евгеньевич','1qz4kw@mail.com','gynQMT'),(3,'Авторизированный клиент','Михайлюк','Анна','Вячеславовна','5d4zbu@tutanota.com','rwVDh9'),(4,'Администратор','Никифорова','Весения','Николаевна','94d5ous@gmail.com','uzWC67'),(5,'Администратор','Одинцов','Серафим','Артёмович','yzls62@outlook.com','JlFRCZ'),(6,'Администратор','Сазонов','Руслан','Германович','uth4iz@mail.com','2L6KZG'),(7,'Авторизированный клиент','Ситдикова','Елена','Анатольевна','ptec8ym@yahoo.com','LdNyos'),(8,'Менеджер','Старикова','Елена','Павловна','wpmrc3do@tutanota.com','RSbvHv'),(9,'Авторизированный клиент','Старикова','Елена','Павловна','4np6se@mail.com','AtnDjr'),(10,'Менеджер','Степанов','Михаил','Артёмович','1diph5e@tutanota.com','8ntwUp');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-03  8:21:30
