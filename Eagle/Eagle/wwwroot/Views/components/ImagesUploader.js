import React from 'react'
import { Darkroom, Canvas, History, Toolbar, FilePicker, CropMenu } from 'react-darkroom';
import {Transform} from '../components/ImgTransform';
import Gallery from '../components/Gallery';
import {Button,Input, Row, Col,Radio,InputNumber,Form, Upload, Icon} from 'antd';
export  class ImagesUploader extends React.Component{
  constructor(props) {
    super(props);
    this.state = {
      hasFile : this.props.hasFile,
      fileSource : [],
      CanvasWidth : this.props.width,
      CanvasHeight : this.props.height,
      crop : false,
      tempimg : this.props.src,
      fileName : null
    }
  }
  //click close Button
  onClickClose = (index, e) => {
    e.preventDefault();
    var file = this.state.fileSource;
    file.splice(index, 1);
    this.setState({
      fileSource : file
    });
  }
  //read img
  readFile = (file, done) => {
    var reader = new FileReader();
    reader.onload = e => done(e.target.result);
    reader.readAsDataURL(file);
  }
  //when input a image in image component
    onFileChange = (e) => {
      this.setState({
        fileName:e.target.files[0]
      });
      this.readFile(e.target.files[0], file => {
        this.setState({
          hasFile : true,
          tempimg : file,
          CanvasWidth : 300,
          CanvasHeight : 300,
          crop : false,
          fileSource : [],
        })
      })
    }
    //upload img
    upload = () => {
      this.setState((prevState) => {
        fileSource : prevState.fileSource.push({'src':prevState.tempimg, 'width':50, 'height':50})
      });
      this.props.callbackParent(this.state);
    }


    render() {
      //select image
      let selectFile = () => {
        this.refs.fileselect.click();
      };
      //confirm editting
      let cropConfirm = () => {
        let {x, y, width, height} = this.refs.canvasWrapper.cropBox;
        let newImage = Transform.cropImage(this.state.tempimg, {x, y, width, height}, { width: 300, height: 300 })
          .then(image => this.setState({tempimg : image, crop : false}));
        if(this.props.hasFile) this.upload();
      };
      //start to edit
      let cropEdit = () => {
        this.setState({crop : true});
      }

      const CanvasStyle={
          border:'1px solid red'
      }
      return (
        <div>
          <Darkroom>
            <Toolbar>
              <Button style={{display:this.props.display}} onClick={selectFile} data-tipsy="Select Image" type="primary">
                <Icon type="upload" />{this.state.fileSource.length > 0 ? 'Upload Another Photo' : 'Upload Photo'}
                <input type="file" ref="fileselect" onChange={this.onFileChange} style={{display: 'none'}}/>
              </Button>
              <CropMenu isCropping={this.state.hasFile}>
                <Button data-showOnlyWhen='croppingIsOn' onClick={cropEdit}>
                  Edit
                </Button>
                <Button disabled={!this.state.crop} data-showOnlyWhen='croppingIsOn' onClick={cropConfirm}>
                  Confirm Edit
                </Button>
                  <Button style={{display:this.props.display}} data-showOnlyWhen='croppingIsOn' onClick={this.upload}>
                    Upload
                  </Button>
              </CropMenu>
            </Toolbar>
                <Canvas ref="canvasWrapper" crop={this.state.crop} source={this.state.tempimg}
                        styles={CanvasStyle} angle={0} width={this.state.CanvasWidth}
                        height={this.state.CanvasHeight}>
                </Canvas>
          </Darkroom>
          {
            this.props.canUpload &&
            <Gallery photos={this.state.fileSource} cols={5} onClickPhoto={this.onClickPhoto} onClickClose={this.onClickClose}/>
          }
        </div>
      )
    }
}

export default ImagesUploader;
