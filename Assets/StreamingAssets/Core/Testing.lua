function onInteract()
    x, y = tank.GetPosition()
    -- core.Print("X: "..x.." Y: "..y)
end

counter = 0
total = 0
function onUpdate(dt)
    counter = counter + dt
    if counter >= 1 and total < 4 then
	player.TakeDamage(1,true)
	counter = 0
	total = total + 1
    end
end

function CreateTank()
    tankParams = {
	name = "tank1",
	image = __dir.."/tank.png",
	pixelsPerUnit = 270,
	x = 2,
	y = 5,
	collision = true
    }
    tank = core.Create(tankParams)
    -- tank.Bind("onInteract",onInteract)
    tank.Bind("onUpdate", onUpdate)
end



function onLoad(name)
    if name == "Town1" then
	CreateTank()
	core.TestData({
		ranged = true,
		amount = 10.4,
		type = "magic"
	})
    end
end

