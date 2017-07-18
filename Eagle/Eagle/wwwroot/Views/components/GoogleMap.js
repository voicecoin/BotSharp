import React from 'react';
import Map, {InfoWindow, Marker} from 'google-maps-react';

export class GoogleMap extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      lat : this.props.lat,
      lng : this.props.lng,
      activeMarker:{}
    };

    this.onClick = this.onClick.bind(this);
    this.onMarkerClick = this.onMarkerClick.bind(this);
  }

  render() {

    const containerStyle = {
      width: '100%',
      height: '250px',
      display: 'inline-block',
      top:'50px',
      left:'0px'
    }

    return(
        <Map google={window.google} containerStyle={containerStyle} initialCenter={{lat:41.8987699, lng:-87.62291679999998}} center={{lat:this.props.initialLat, lng:this.props.initialLng}} zoom={16} onClick={this.onClick}>
          <Marker position={{lat: this.props.lat, lng: this.props.lng}} onClick={this.onMarkerClick} name={this.props.address}/>
            <InfoWindow
              marker={this.state.activeMarker}
              visible={true}>
              <div>
                <h4>{this.props.address}</h4>
                <a href={`http://maps.google.com/?q=${this.props.address}`} target='_blank'>View on Google Maps</a>
              </div>
            </InfoWindow>
        </Map>
    )
  }
  onClick(mapProps, map, clickEvent){
    this.setState({
      lat : clickEvent.latLng.lat(),
      lng : clickEvent.latLng.lng()
    });
  }

  onMarkerClick(props, marker, clickEvent) {
    this.setState({
      lat : clickEvent.latLng.lat(),
      lng : clickEvent.latLng.lng(),
      activeMarker:marker
    });
  }
}

export default GoogleMap
