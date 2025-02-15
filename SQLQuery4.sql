CREATE TABLE user_portfolio (
    id INT IDENTITY(1,1) PRIMARY KEY,
    description TEXT NOT NULL, 
    user_id INT, 
    created_at DATETIME DEFAULT current_timestamp, 
    updated_at DATETIME DEFAULT current_timestamp,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
);
select * from user_portfolio;
use pharmacy;
select * from Users;
INSERT INTO user_portfolio (description, user_id)
VALUES ('This is a sample description for the user portfolio.', 5);

select * from user_portfolio where user_id=5