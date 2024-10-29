--trackage elements
INSERT INTO TrackageElement DEFAULT VALUES
INSERT INTO TrackageElement DEFAULT VALUES
INSERT INTO TrackageElement DEFAULT VALUES
INSERT INTO TrackageElement DEFAULT VALUES
INSERT INTO TrackageElement DEFAULT VALUES
INSERT INTO TrackageElement DEFAULT VALUES
UPDATE TrackageElement SET LeftSideElementId = NULL, RightSideElementId = 2 WHERE TrackageElementId = 1
UPDATE TrackageElement SET LeftSideElementId = NULL, RightSideElementId = NULL WHERE TrackageElementId = 2
UPDATE TrackageElement SET LeftSideElementId = 2, RightSideElementId = 5 WHERE TrackageElementId = 3
UPDATE TrackageElement SET LeftSideElementId = 2, RightSideElementId = NULL WHERE TrackageElementId = 4
UPDATE TrackageElement SET LeftSideElementId = 3, RightSideElementId = 6 WHERE TrackageElementId = 5
UPDATE TrackageElement SET LeftSideElementId = 5, RightSideElementId = NULL WHERE TrackageElementId = 6

--switches
INSERT INTO Switch VALUES(2)

--tracks
INSERT INTO Track VALUES
(1,50,30,30,3,0,1,1,0),
(3,40,30,30,4,0,2,1,0),
(4,35,30,30,4,0,3,1,0),
(5,20,20,20,2,40,2,1,1),
(6,30,30,80,5,60,2,1,2)

--signs
INSERT INTO Signs VALUES
(4, 0, 1,30),
(4, 5, 1, 50),
(4, 15, 1, 15),
(4, 10, 0, 30)

--signals
INSERT INTO TrackSignals VALUES
(1, 15, 1),
(1, 30, 1),
(3, 0, 1),
(3, 15, 1),
(3, 25, 0),
(5, 0, 1),
(5, 10, 1),
(5, 10, 0),
(6, 5, 1),
(6, 5, 0)