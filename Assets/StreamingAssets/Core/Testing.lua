function onInteract()
    core.ShowDialogue("Hello, {S_playerName}! It's good to see you!", "fast")
end

function onLoad(name)
    core.Print(name)
    if name == "Town1" then
	tankParams = {
	    name = "tank1",
	    image = __dir.."/tank.png",
	    pixelsPerUnit = 270,
	    x = 3,
	    collision = true
	}
        tank = core.Create(tankParams)
        tank.Bind("onInteract",onInteract)
    end
end

