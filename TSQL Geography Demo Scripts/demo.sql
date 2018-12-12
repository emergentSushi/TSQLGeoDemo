--- 0 MakeValid
UPDATE Zones
	SET geog = geog.MakeValid()

-- Simplify the polygons so that queries run faster
UPDATE Zones
	SET geog = geog.Reduce(15)

--bad zones
--delete from Zones
--where [name] = 'Area Outside Regional Council Constituency' 
--OR [name] = 'Southern Constituency'
--OR [name] = 'Hokonui Constituency'
--OR [name] = 'Dunstan Constituency'
--OR [name] = 'Eastern-Dome Constituency'
--OR [name] = 'South Canterbury Constituency'
--OR [name] = 'Mid-Canterbury Constituency'
--OR [name] = 'Tararua Constituency'
--OR [name] = 'Horowhenua-Kairanga Constituency'
--OR [name] = 'Palmerston North Constituency'
--OR [name] = 'Manawatu-Rangitikei Constituency'
--OR [name] = 'Ruapehu Constituency'
--OR [name] = 'South Taranaki Constituency'
--OR [name] = 'Stratford Constituency'
--OR [name] = 'Hastings Constituency'
--OR [name] = 'Central Hawke''s Bay Constituency'
--OR [name] = 'Whangarei Urban Constituency'

--OR [name] = 'Wairarapa Constituency'
--OR [name] = 'Grey Constituency'
--OR [name] = 'North Canterbury Constituency'
--OR [name] = 'Upper Hutt Constituency'
--OR [name] = 'Wanganui Constituency'
--OR [name] = 'Napier Constituency'
--OR [name] = 'Hamilton Constituency'
--OR [name] = 'Taupo-Rotorua Constituency'

--OR [name] = 'Waikato Constituency'

--- 1 Basic Spatial data display
-- far north, the ID's mean nothing
select * from Zones where id < 9



--- 2 Get a zone based on a point
DECLARE @kawakawa geography = geography::Point(-35.3795, 174.0646, 4326)
select name, geog from Zones where geog.STIntersects(@kawakawa) = 1




--- 3 Other basic ops
DECLARE @whangarei geography = geography::Point(-35.7251, 174.3237, 4326)
DECLARE @auckland geography = geography::Point(-36.848461, 174.763336, 4326)
select @whangarei.STDistance(@auckland) / 1000 --straight line distance between 2 points



--- 4 What exactly is a Geography type?
DECLARE @foo geography = geography::STGeomFromText('POLYGON EMPTY', 4326)
select @foo