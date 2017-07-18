const Filter = require('image-filters');
export var blur = function(img) {
  var ele = new Filter({url : 'https://static.pexels.com/photos/104827/cat-pet-animal-domestic-104827.jpeg'});
  Filter .grayscale()
      .apply()

}
