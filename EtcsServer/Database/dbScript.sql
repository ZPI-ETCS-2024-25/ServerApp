--trackage elements
SET IDENTITY_INSERT TrackageElement ON;
INSERT INTO TrackageElement(TrackageElementId, LeftSideElementId, RightSideElementId) VALUES(1, NULL, NULL)
INSERT INTO TrackageElement(TrackageElementId, LeftSideElementId, RightSideElementId) VALUES(2, NULL, NULL)
INSERT INTO TrackageElement(TrackageElementId, LeftSideElementId, RightSideElementId) VALUES(3, NULL, NULL)
INSERT INTO TrackageElement(TrackageElementId, LeftSideElementId, RightSideElementId) VALUES(4, NULL, NULL)
INSERT INTO TrackageElement(TrackageElementId, LeftSideElementId, RightSideElementId) VALUES(5, NULL, NULL)
INSERT INTO TrackageElement(TrackageElementId, LeftSideElementId, RightSideElementId) VALUES(6, NULL, NULL)
SET IDENTITY_INSERT TrackageElement OFF;
UPDATE TrackageElement SET LeftSideElementId = NULL, RightSideElementId = 2 WHERE TrackageElementId = 1
UPDATE TrackageElement SET LeftSideElementId = NULL, RightSideElementId = NULL WHERE TrackageElementId = 2
UPDATE TrackageElement SET LeftSideElementId = 2, RightSideElementId = 5 WHERE TrackageElementId = 3
UPDATE TrackageElement SET LeftSideElementId = 2, RightSideElementId = NULL WHERE TrackageElementId = 4
UPDATE TrackageElement SET LeftSideElementId = 3, RightSideElementId = 6 WHERE TrackageElementId = 5
UPDATE TrackageElement SET LeftSideElementId = 5, RightSideElementId = NULL WHERE TrackageElementId = 6

--tracks
INSERT INTO Track(TrackageElementId, [Length], MaxUpSpeedMps, MaxDownSpeedMps, Gradient, Kilometer, LineNumber, TrackNumber, TrackPosition) VALUES
(1,50,30,30,3,0,1,1,0),
(3,40,30,30,4,0,2,1,0),
(4,35,30,30,4,0,3,1,0),
(5,20,20,20,2,40,2,1,1),
(6,30,30,80,5,60,2,1,2)

--switches
INSERT INTO Switch VALUES(2)
INSERT INTO TrackSwitches(SwitchId, TrackFromId, TrackToId, MaxSpeedMps) VALUES
(2, 1, 3, 20),
(2, 1, 4, 50)

--signs
INSERT INTO Signs(TrackId, DistanceFromTrackStart, IsFacedUp, MaxSpeed) VALUES
(4, 0, 1,30),
(4, 5, 1, 50),
(4, 15, 1, 15),
(4, 10, 0, 30)

--signals
INSERT INTO TrackSignals(TrackId, DistanceFromTrackStart, IsFacedUp) VALUES
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