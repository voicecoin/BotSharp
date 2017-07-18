import React from 'react'
import PlacesAutocomplete, {geocodeByAddress, getLatLng} from 'react-places-autocomplete';
         //define style for google autocomplete Component
         const defaultStyles = {
           root: {
             position: 'relative',
             paddingBottom: '0px',
             float:'left',
             width:'100%'
           },
           input: {
             display: 'inline-block',
             width: '100%',
             height:'2.8em',
             border : '1px solid #d9d9d9',
             borderRadius : '4px',
             fontSize: '12px'
           },
           autocompleteContainer: {
             position: 'absolute',
             top: '100%',
             backgroundColor:'red',
             border:'none',
            "box-shadow":" 0 1px 6px rgba(0,0,0,.2)",
             width: '100%',
             zIndex:9999999
           },
           autocompleteItem: {
             backgroundColor: '#FFF',
             padding: '10px',
             color: '#555555',
             cursor: 'pointer'
           },
           autocompleteItemActive: {
             backgroundColor: '#fafafa'
           }
         }
export class GooglePlacesAutocomplete extends React.Component{
    constructor(props){
        super(props)
        this.state={
             address : '',
             initialLat : 41.8987699,
             initialLng : -87.62291679999998,
             lat : null,
             lng : null,
        }
        this.onSelect = this.onSelect.bind(this);
        this.onChange = this.onChange.bind(this);
    }

        //when select an address Google autocomplete component
        onSelect(place, id) {
          geocodeByAddress(place)
          .then(result => {
            getLatLng(result[0]).then(data => {
              this.setState({
                address : place,
                lat : data.lat,
                lng : data.lng,
                initialLat : data.lat,
                initialLng : data.lng
              }
             
            );
            this.props.callbackParent(this.state)
          })})
          .catch(err => {
            console.log('Error Occured', err);
          })
        }
        //when change input in google autocomplete
        onChange(newaddress) {
          if(newaddress.length == 0) {
            this.setState({
              initialLat : 41.8987699,
              initialLng : -87.62291679999998,
              lat : null,
              lng : null
            })
          }
          this.setState({
            address : newaddress
          })

        }    
        render(){
        const inputParas = {
           value : this.state.address,
           onChange : this.onChange
         };
            return(
                 <PlacesAutocomplete inputProps={inputParas} onSelect={this.onSelect} onEnterKeyDown={this.onSelect} styles={defaultStyles}/>    
            )
        }
}
export default GooglePlacesAutocomplete;