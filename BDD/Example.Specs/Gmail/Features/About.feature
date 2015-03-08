@GMailAbout
Feature: About GMail
	In order to provide details about gmail
	As google
	I want to welcome users to gmail

Scenario: English About Page Talks About Security	
	Given English About page is loaded
	 When I go to features
	  And I click on More Features
	 Then HTTPS Security is mentioned