function onInteract()
    x, y = tank.GetPosition()
    core.Print("X: "..x.." Y: "..y)
end

function onUpdate(dt)
    core.Print("Delta Time is : " .. dt)
end


function onLoad(name)
    core.Print(name)
    if name == "Town1" then
	tankParams = {
	    name = "tank1",
	    image = __dir.."/tank.png",
	    pixelsPerUnit = 270,
	    x = 2,
	    y = 5,
	    collision = true
	}
        tank = core.Create(tankParams)
        tank.Bind("onInteract",onInteract)
	tank.Bind("onUpdate", onUpdate)
    end
end

