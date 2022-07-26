
import os
import random
from random import randint
import pygame
import time
from pygame import QUIT, KEYDOWN, K_ESCAPE, K_SPACE, K_LEFT, K_RIGHT, K_UP, K_DOWN, K_SPACE, K_BACKSPACE, K_w, K_s

class Data(object):
	def __init__(self):
		self.screenHeight = 400
		self.screenWidth = 400
		self.movingPowerups = True
		self.movingApples = True
		self.slowTime = False
		self.player = Player(self)
		self.bullets = []
		self.apples = []
		self.tails = []
		self.powerups = []
		self.frameCount = 0
		self.powerupInterval = 200
		self.nukes = []
		self.starInterval = 20
		self.stars = []
		self.game = Game(self)
		self.session = Session(self)
		self.playtime = 0
		self.FPS = 60
		self.clock = pygame.time.Clock()
		self.screen = pygame.display.set_mode((self.screenHeight, self.screenWidth))

    
	def resetDetails(self):
		self.player = Player(self)
		self.bullets = []
		self.apples = [Apple(self)]
		self.tails = []
		self.powerups = []
		self.frameCount = 0
		self.powerupInterval = 200
		self.nukes = []
		self.playtime = 0

class Player(object):
	def __init__(self, data):
		self.superstar = False
		self.superstarCount = 0
		self.height = 11
		self.width = 11
		self.score = 0
		self.speed = 2
		self.trailInterval = 5
		self.rect = pygame.Rect((data.screenWidth / 2),(data.screenHeight / 2),self.width,self.height)
		self.direction = 'NONE'
		self.dx = 0
		self.dy = 0
		self.trails = []

	def move(self, data):
		self.rect.x += self.dx
		self.rect.y += self.dy

		if self.rect.left > data.screenWidth:
			self.rect.right = 0
		if self.rect.right < 0:
			self.rect.left = data.screenWidth
		if self.rect.bottom < 0:
			self.rect.top = data.screenHeight
		if self.rect.top > data.screenHeight:
			self.rect.bottom = 0

		if self.superstar == True:
			self.superstarCount += 1
			if self.superstarCount >= 190:
				if self.superstarCount % 20 == 0:
					self.width += 10
					self.height += 10
					self.rect.x -= 5
					self.rect.y -= 5
				elif self.superstarCount % 10 == 0:
					self.width -= 10
					self.height -= 10
					self.rect.x += 5
					self.rect.y += 5
					if self.superstarCount == 310:
						self.superstar = False
						self.superstarCount = 0

		for apple in data.apples:
			if self.rect.colliderect(apple.rect):
				data.apples.remove(apple)
				self.score += 10
				Apple(data)
				Tail(data)

		for tail in data.tails:
			if self.rect.colliderect(tail.rect):
				if self.superstar == False:
					data.session.running = False
					print('Final Score: {0:.1f}		Time: {1:.2f}'.format(data.player.score,data.playtime))
				else:
					data.tails.remove(tail)

		for powerup in data.powerups:
			if self.rect.colliderect(powerup.rect):
				if powerup.type == 1:
					Nuke(data, powerup.rect.x, powerup.rect.y)
					data.powerups.remove(powerup)
				if powerup.type == 2:
					data.powerups.remove(powerup)	
					self.superstarFunc()				

	def superstarFunc(self):
		if self.superstar != True:
			self.superstar = True
			self.width += 10
			self.height += 10
			self.rect.x -= 5
			self.rect.y -= 5

	def checkHighscore(self, data):
		if self.score > data.session.highscore:
			data.session.highscore = self.score

class Trail(object):
	def __init__(self, data):
		data.player.trails.append(self)
		self.height = (data.player.height - 1) / 2
		if self.height % 2 == 0:
			self.height -= 1
		self.width = (data.player.width - 1) / 2
		if self.width % 2 == 0:
			self.width -= 1
		self.x = data.player.rect.x + ((data.player.height - self.width) / 2)
		self.y = data.player.rect.y + ((data.player.height - self.width) / 2)
		self.dx = -1 *data.player.dx
		self.dy = -0.1 *data.player.dy
		self.rect = pygame.Rect(self.x,self.y,self.width,self.height)
		self.age = 0

	def move(self, data):
		self.age += 1
		if self.age % 5 == 0:
			self.width -= 2
			self.height -= 2
			self.rect.x += 1
			self.rect.y += 1

		if self.width <= 0:
			data.player.trails.remove(self)


class Bullet(object):
	def __init__(self, data):
		data.bullets.append(self)
		self.height = 4
		self.width = 4
		x = data.player.rect.x + 3
		y = data.player.rect.y + 3
		self.dx = 2.25*data.player.dx
		self.dy = 2.25*data.player.dy
		self.rect = pygame.Rect(x,y,self.width,self.height)

	def move(self, data):
		self.rect.x += self.dx
		self.rect.y += self.dy

		for apple in data.apples:
			if self.rect.colliderect(apple.rect):
				data.apples.remove(apple)
				data.player.score += 1
				apple = Apple(data)
				Tail(data)

		for tail in data.tails:
			if self.rect.colliderect(tail.rect):
				data.tails.remove(tail)

class Star(object):
	def __init__(self, data):
		data.stars.append(self)
		self.frameCount = 0
		self.ageInterval = 5
		self.age = 0
		self.x = randint(5, data.screenWidth - 5)
		self.y = randint(5, data.screenHeight - 5)
		self.shapes = []

	def move(self,data):
		self.x -= data.player.dx / 15
		self.y -= data.player.dy / 15

	def sparkle(self, data):
		self.frameCount += 1
		if self.frameCount % self.ageInterval == 0:
			self.age += 1 
		self.shapes = []
		if self.age == 1 or self.age == 9:
			self.shapes.append(pygame.Rect(self.x, self.y, 1, 1))
		elif self.age == 2 or self.age == 8:
			self.shapes.append(pygame.Rect(self.x - 1, self.y, 3, 1))
			self.shapes.append(pygame.Rect(self.x, self.y - 1, 1, 3))
		elif self.age == 3 or self.age == 7:
			self.shapes.append(pygame.Rect(self.x - 2, self.y, 5, 1))
			self.shapes.append(pygame.Rect(self.x, self.y - 2, 1, 5))
		elif self.age == 4 or self.age == 5  or self.age == 6:
			self.shapes.append(pygame.Rect(self.x - 3, self.y, 7, 1))
			self.shapes.append(pygame.Rect(self.x, self.y - 3, 1, 7))
			self.shapes.append(pygame.Rect(self.x - 1, self.y - 1, 3, 3))
		elif self.age == 10:
			data.stars.remove(self)

class PowerUp(object):
	def __init__(self, data):
		self.direction = 0
		self.dx = 0; self.dy = 0
		if data.movingPowerups == True:
			self.direction = self.getRandomDirection(data)
		self.type = randint(1,2)
		self.height = 10
		self.width = 10
		self.x = randint(2,38)*10
		self.y = randint(2,38)*10
		data.powerups.append(self)
		self.shape = [self.x, self.y], [self.x + 4, self.y + 2], [self.x + 8, self.y], [self.x + 6, self.y + 4], [self.x + 8, self.y + 8],[self.x + 4, self.y + 6], [self.x, self.y + 8], [self.x + 2, self.y + 4]
		self.rect = pygame.Rect(self.x,self.y,self.width,self.height)

	def getRandomDirection(self, data):
		direction = randint(1,4)
		if direction == 1:
			self.dx = data.player.speed
		if direction == 2:
			self.dy = data.player.speed
		if direction == 3:
			self.dx = -data.player.speed
		if direction == 4:
			self.dy = -data.player.speed
		return direction

	def move(self, data):
		if self.direction % 2 == 0 and self.direction != 0:
			self.dx += randint(-1,1)
			if self.dx > 5:
				self.dx = 5
			elif self.dx < -5:
				self.dx = -5
			self.x += self.dx
			self.y += self.dy
		elif self.direction > 0:
			self.dy += randint(-1,1)
			if self.dy > 5:
				self.dy = 5
			elif self.dy < -5:
				self.dy = -5
			self.x += self.dx
			self.y += self.dy			

		self.shape = [self.x, self.y], [self.x + 4, self.y + 2], [self.x + 8, self.y], [self.x + 6, self.y + 4], [self.x + 8, self.y + 8],[self.x + 4, self.y + 6], [self.x, self.y + 8], [self.x + 2, self.y + 4]
		self.rect = pygame.Rect(self.x,self.y,self.width,self.height)

		if self.rect.left > data.screenWidth:
			self.rect.right = 0
		if self.rect.right < 0:
			self.rect.left = data.screenWidth
		if self.rect.bottom < 0:
			self.rect.top = data.screenHeight
		if self.rect.top > data.screenHeight:
			self.rect.bottom = 0


		
class Nuke(object):
	def __init__(self, data, x, y):
		self.expand = True
		self.width = 10
		self.height = 10
		self.x = x
		self.y = y
		self.rect = pygame.Rect(self.x, self.y, self.width, self.height)
		self.count = 0
		data.nukes.append(self)
		self.shockwaveCount = 0
		self.shockwaveGap = 5
		self.shockwaveSize = 1
		self.shockwaveInterval = 1
		self.shockwaves = []
		self.shockwaveLevel = 0

	def grow(self, data):
		self.count += 1
		if self.expand == True:
			if self.count % 3 == 0:
				self.rect.width += 1
				self.rect.height += 1
			if self.count % 6 == 0:
				self.rect.x -= .5
				self.rect.y -= .5

		if self.count > 266:
			self.expand = False
			self.createShockwaves(data)

		if self.count > 272:
			data.nukes.remove(self)

		for tail in data.tails:
			if self.rect.colliderect(tail.rect):
				data.tails.remove(tail)

	def createShockwaves(self, data):
		self.shockwaveCount += 1
		if self.shockwaveCount % self.shockwaveInterval == 0:
			self.shockwaves = []
			self.shockwaveLevel += 1
			self.shockwaves.append(pygame.Rect(self.rect.x, self.rect.y - ((self.shockwaveLevel * 3) * self.shockwaveGap), self.rect.width, self.shockwaveSize))
			self.shockwaves.append(pygame.Rect(self.rect.x + self.rect.width + ((self.shockwaveLevel * 3) * self.shockwaveGap), self.rect.y, self.shockwaveSize, self.rect.height))
			self.shockwaves.append(pygame.Rect(self.rect.x, self.rect.y + self.rect.height + ((self.shockwaveLevel * 3) * self.shockwaveGap), self.rect.width, self.shockwaveSize))
			self.shockwaves.append(pygame.Rect(self.rect.x - ((self.shockwaveLevel * 3) * self.shockwaveGap) - self.shockwaveSize, self.rect.y, self.shockwaveSize, self.rect.height))
			self.rect.x += 2 * self.shockwaveGap
			self.rect.y += 2 * self.shockwaveGap
			self.rect.height -= 4 * self.shockwaveGap
			self.rect.width -= 4 * self.shockwaveGap
			#data.clock.tick(1)

class Tail(object):
	def __init__(self, data):
		data.tails.append(self)
		self.height = 7
		self.width = 7
		if data.player.dx == 0:
			self.dy = -2*data.player.dy
			self.dx = 0
		if data.player.dy == 0:
			self.dx = -2*data.player.dx
			self.dy = 0
		x = (data.player.rect.x - (data.player.width / 2) * data.player.dx) + 2
		y = (data.player.rect.y - (data.player.height / 2) * data.player.dy) + 2
		self.rect = pygame.Rect(x,y,self.width,self.height)

	def move(self, data):
		self.rect.x += self.dx
		self.rect.y += self.dy

		if self.rect.left > data.screenWidth:
			self.rect.right = 0
		if self.rect.right < 0:
			self.rect.left = data.screenWidth
		if self.rect.bottom < 0:
			self.rect.top = data.screenHeight
		if self.rect.top > data.screenHeight:
			self.rect.bottom = 0

class Apple(object):
	def __init__(self, data):
		self.dx = 0; self.dy = 0
		if data.movingApples == True and data.player.score != 0:
			self.getRandomDirection(data)
		height = 10
		width = 10
		data.apples.append(self)
		x = randint(2,38)*10
		y = randint(2,38)*10
		self.rect = pygame.Rect(x,y,width,height)

	def move(self, data):
		self.rect.x += self.dx
		self.rect.y += self.dy

		if self.rect.left > data.screenWidth:
			self.rect.right = 0
		if self.rect.right < 0:
			self.rect.left = data.screenWidth
		if self.rect.bottom < 0:
			self.rect.top = data.screenHeight
		if self.rect.top > data.screenHeight:
			self.rect.bottom = 0

	def getRandomDirection(self, data):
		direction = randint(1,4)
		if direction == 1:
			self.dx = data.player.speed
		elif direction == 2:
			self.dx = -data.player.speed
		elif direction == 3:
			self.dy = data.player.speed
		elif direction == 4:
			self.dy = -data.player.speed

class Game(object):
	def __init__(self, data):
		self.running = True

class Session(object):
	def __init__(self, data):
		self.running = True
		self.highscore = 0


def main():
	data = Data()
	runGame(data)
	pygame.quit()
	print('Final Score: {0:.1f}		Time: {1:.2f}'.format(data.player.score,data.playtime))

def startGame(data):
	for e in pygame.event.get():
		if e.type == QUIT:
			data.session.running = False
			data.game.running = False
		if e.type == KEYDOWN:
			if e.key == K_SPACE:
				data.session.running = True
				data.resetDetails()
			if e.key == K_ESCAPE:
				data.session.running = False
				data.game.running = False

def starDisplay(data):
	data.frameCount += 1
	if data.frameCount % data.starInterval == 0:
		Star(data)
	for star in data.stars:
		star.sparkle(data)

def display(data):
		if data.slowTime == True:
			data.clock.tick(1)
		data.screen.fill((54,8,64))
		for star in data.stars:
			for shape in star.shapes:
				pygame.draw.rect(data.screen, (213,181,206), shape)
		for tail in data.tails:
			#pygame.draw.rect(data.screen, (229, 57, 53), tail.rect)
			pygame.draw.rect(data.screen, (245, 32, 108), tail.rect)
		for apple in data.apples:
			pygame.draw.rect(data.screen, (77, 208, 225), apple.rect)
		for bullet in data.bullets:
			pygame.draw.rect(data.screen, (0,255,0), bullet.rect)
		for trail in data.player.trails:
			trailPoly = [trail.rect.x + (trail.width - 1) / 2, trail.rect.y], [trail.rect.x + (trail.width - 1), trail.rect.y + (trail.height - 1) / 2], [trail.rect.x + (trail.width - 1) / 2, trail.rect.y + (trail.height - 1)], [trail.rect.x, trail.rect.y + (trail.height - 1) / 2]
			pygame.draw.polygon(data.screen, (220, 220, 220), trailPoly)
		for powerup in data.powerups:
			#colour = (randint(1,255),randint(1,255),randint(1,255))
			#pygame.draw.rect(data.screen, colour, powerup.rect)
			colour = (randint(1,255),randint(1,255),randint(1,255))
			pygame.draw.polygon(data.screen, colour, powerup.shape)
		for nuke in data.nukes:
			if nuke.expand == True:
				pygame.draw.rect(data.screen, (255,125,0), nuke.rect)
			for shockwave in nuke.shockwaves:
				pygame.draw.rect(data.screen, (255,125,0), shockwave)
		#pygame.draw.rect(data.screen, (0, 255, 0), data.player.rect)
		#pygame.draw.circle(data.screen, (255, 255, 0), [data.player.rect.x + 5, data.player.rect.y + 5], 5)
		playerPoly = [data.player.rect.x + (data.player.width - 1) / 2, data.player.rect.y], [data.player.rect.x + (data.player.width - 1), data.player.rect.y + (data.player.height - 1) / 2], [data.player.rect.x + (data.player.width - 1) / 2, data.player.rect.y + (data.player.height - 1)], [data.player.rect.x, data.player.rect.y + (data.player.height - 1) / 2]
		pygame.draw.polygon(data.screen, (255, 215, 64), playerPoly)
						
		caption = ('Backfire -- High: ' + str(data.session.highscore) + ' / Score: ' + str(data.player.score) + ' / Time: ' + str(round(data.playtime)))
		pygame.display.set_caption(caption)
		pygame.display.flip()

def runGame(data):
	data.apple = [Apple(data)]
	while data.game.running:
		startGame(data)
		starDisplay(data)
		display(data)
		while data.session.running:
			runSession(data)

def playerAction(data):
	for e in pygame.event.get():
		if e.type == QUIT:
			data.session.running = False
			data.game.running = False
		if e.type == KEYDOWN:
			if e.key == K_ESCAPE:
				data.session.running = False
				data.game.running = False
			elif e.key == K_LEFT:
				data.player.dy = 0
				data.player.dx = -data.player.speed
			elif e.key == K_RIGHT:
				data.player.dy = 0
				data.player.dx = data.player.speed
			elif e.key == K_UP:
				data.player.dx = 0
				data.player.dy = -data.player.speed
			elif e.key == K_DOWN:
				data.player.dx = 0
				data.player.dy = data.player.speed
			elif e.key == K_SPACE:
				for bullet in data.bullets:
					data.bullets.remove(bullet)
				Bullet(data)
			elif e.key == K_BACKSPACE:
				data.movingApples = True
			elif e.key == K_s:
				if data.slowTime == True:
					data.slowTime = False
				else:
					data.slowTime = True
			elif e.key == K_w:
				PowerUp(data)

def runSession(data):
	while data.session.running:
		milliseconds = data.clock.tick(data.FPS)
		data.playtime += milliseconds / 1000.0
	
		playerAction(data)
	
		data.player.move(data)
	
		for tail in data.tails:
			tail.move(data)
	
		for bullet in data.bullets:
			bullet.move(data)
		
		for powerup in data.powerups:
			powerup.move(data)
	
		for nuke in data.nukes:
			nuke.grow(data)
		
		for apple in data.apples:
			apple.move(data)
		
		for trail in data.player.trails:
			trail.move(data)

		for star in data.stars:
			star.move(data)
			star.sparkle(data)

		data.frameCount += 1
		if data.frameCount % 1000 == 0:
			if len(data.powerups) == 0:
				PowerUp(data)
		
		if data.frameCount % data.player.trailInterval == 0:
			Trail(data)

		if data.frameCount % data.starInterval == 0:
			Star(data)

		data.player.checkHighscore(data)
		for apple in data.apples:
			apple.move(data)
		
		display(data)



main()